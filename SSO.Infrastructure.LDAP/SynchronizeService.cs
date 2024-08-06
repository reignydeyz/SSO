using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using SSO.Infrastructure.LDAP.Models;
using SSO.Infrastructure.Settings.Enums;
using System.DirectoryServices;

namespace SSO.Infrastructure.LDAP
{
    public class SynchronizeService
    {
        readonly IRealmRepository _realmRepository;
        readonly IRealmUserRepository _realmUserRepository;
        readonly IGroupRepository _groupRepository;
        readonly IGroupUserRepository _groupUserRepository;
        readonly IUserRepository _userRepository;

        LDAPSettings _ldapSettings;
        string _ldapConnectionString;
        Guid _realmId;

        public SynchronizeService(IRealmRepository realmRepository, IRealmUserRepository realmUserRepository, IServiceProvider serviceProvider)
        {
            _realmRepository = realmRepository;
            _realmUserRepository = realmUserRepository;
            _groupRepository = serviceProvider.GetRequiredService<Management.GroupRepository>();
            _groupUserRepository = serviceProvider.GetRequiredService<Management.GroupUserRepository>();
            _userRepository = serviceProvider.GetRequiredService<Management.UserRepository>();
        }

        public async Task Begin(Guid realmId)
        {
            var realm = await _realmRepository.FindOne(x => x.RealmId == realmId && x.DateInactive == null && x.IdpSettingsCollection.Any(y => y.IdentityProvider == IdentityProvider.LDAP));

            if (realm is not null)
            {
                _ldapSettings = JsonConvert.DeserializeObject<LDAPSettings>(realm.IdpSettingsCollection.First(x => x.IdentityProvider == IdentityProvider.LDAP).Value);
                _ldapConnectionString = $"{(_ldapSettings.UseSSL ? "LDAPS" : "LDAP")}://{_ldapSettings.Server}:{_ldapSettings.Port}/{_ldapSettings.SearchBase}";
                _realmId = realmId;

                var groups = await FetchGroupsFromLDAP();
                await SyncGroupsToDatabase(groups);

                var (users, groupUsers) = await FetchUsersAndGroupUsersFromLDAP();
                await SyncUsersToDatabase(users, groupUsers);

                // Cleanup
                await _userRepository.RemoveRange(x => !x.Realms.Any());
            }
        }

        private async Task<List<Group>> FetchGroupsFromLDAP()
        {
            using (var entry = new DirectoryEntry(_ldapConnectionString, _ldapSettings.Username, _ldapSettings.Password))
            {
                var searcher = CreateDirectorySearcher(entry, "(objectClass=group)", new[] { "cn", "sAMAccountName", "description", "distinguishedName" });
                var groups = await ExecuteSearchAndExtractGroups(searcher);
                return groups;
            }
        }

        private async Task<List<Group>> ExecuteSearchAndExtractGroups(DirectorySearcher searcher)
        {
            var groups = new List<Group>();
            SearchResultCollection results = searcher.FindAll();

            foreach (SearchResult searchResult in results)
            {
                var rec = searchResult.GetDirectoryEntry();
                groups.Add(new Group { GroupId = Guid.NewGuid(), Name = rec.Properties["cn"].Value?.ToString() ?? string.Empty, RealmId = _realmId });
            }

            return groups;
        }

        private async Task SyncGroupsToDatabase(List<Group> groups)
        {
            var toBeAddedGroups = groups.Where(x => !_groupRepository.Any(y => y.Name == x.Name && x.RealmId == _realmId).Result);
            await _groupRepository.AddRange(toBeAddedGroups, false);

            var groupNames = groups.Select(x => x.Name.ToLower());
            await _groupRepository.RemoveRange(x => !groupNames.Contains(x.Name) && x.RealmId == _realmId);
        }

        private async Task<(List<ApplicationUser>, List<Tuple<string, string>>)> FetchUsersAndGroupUsersFromLDAP()
        {
            var users = new List<ApplicationUser>();
            var groupUsers = new List<Tuple<string, string>>();

            using (var entry = new DirectoryEntry(_ldapConnectionString, _ldapSettings.Username, _ldapSettings.Password))
            {
                var searcher = CreateDirectorySearcher(entry, _ldapSettings.SearchFilter);
                searcher.ReferralChasing = ReferralChasingOption.All;
                SearchResultCollection results = searcher.FindAll();

                foreach (SearchResult result in results)
                {
                    if (result.Properties["sAMAccountName"]?.Count > 0 &&
                        result.Properties["givenName"]?.Count > 0 &&
                        result.Properties["sn"]?.Count > 0)
                    {
                        var user = ExtractUserFromSearchResult(result);
                        users.Add(user);

                        await ExtractGroupUsersFromUserEntry(result.GetDirectoryEntry(), user.UserName, groupUsers);
                    }
                }
            }

            return (users, groupUsers);
        }

        private ApplicationUser ExtractUserFromSearchResult(SearchResult result)
        {
            return new ApplicationUser
            {
                UserName = result.Properties["sAMAccountName"][0].ToString(),
                NormalizedUserName = result.Properties["sAMAccountName"][0].ToString().ToUpper(),
                FirstName = result.Properties["givenName"][0].ToString(),
                LastName = result.Properties["sn"][0].ToString(),
                PasswordHash = Guid.NewGuid().ToString()
            };
        }

        private async Task ExtractGroupUsersFromUserEntry(DirectoryEntry userEntry, string userName, List<Tuple<string, string>> groupUsers)
        {
            if (userEntry.Properties.Contains("memberOf"))
            {
                foreach (string groupPath in userEntry.Properties["memberOf"])
                {
                    string groupName = groupPath.Split(',')[0].Substring(3);

                    if (!groupUsers.Exists(x => x.Item1 == groupName && x.Item2 == userName))
                        groupUsers.Add(new Tuple<string, string>(groupName, userName));
                }
            }
        }

        private async Task SyncUsersToDatabase(List<ApplicationUser> users, List<Tuple<string, string>> groupUsers)
        {
            var toBeAddedUsers = users.Where(x => !_userRepository.Any(y => y.UserName == x.UserName).Result);
            await _userRepository.AddRange(toBeAddedUsers, false);

            var usernames = users.Select(x => x.UserName.ToLower());

            var toBeAddedRealmUsers = toBeAddedUsers.Where(x => !_realmUserRepository.Any(y => y.UserId == x.Id && y.RealmId == _realmId).Result)
                .Select(x => new RealmUser { RealmId = _realmId, UserId = x.Id });
            var existingUsersNotInRealm = (await _userRepository.Find(x => usernames.Contains(x.UserName) && !x.Realms.Any(y => y.RealmId == _realmId)))
                .Select(x => new RealmUser { RealmId = _realmId, UserId = x.Id });
            await _realmUserRepository.AddRange(toBeAddedRealmUsers.Union(existingUsersNotInRealm), false);

            var toBeDeletedRealmUsers = (await _realmUserRepository.Find(x => !usernames.Contains(x.User.UserName)))
                .Select(x => new RealmUser { RealmId = _realmId, UserId = x.UserId });
            await _realmUserRepository.RemoveRange(toBeDeletedRealmUsers, false);

            await SyncGroupUsers(groupUsers);
        }

        private async Task SyncGroupUsers(List<Tuple<string, string>> groupUsers)
        {
            var guList = (from gu in groupUsers
                          join u in await _userRepository.Find(x => x.Realms.Any(y => y.RealmId == _realmId)) on gu.Item2 equals u.UserName
                          join g in await _groupRepository.Find(x => x.RealmId == _realmId) on gu.Item1 equals g.Name
                          select new GroupUser
                          {
                              GroupId = g.GroupId,
                              UserId = u.Id
                          });

            var toBeAddedGroupUsers = guList.Where(x => !_groupUserRepository.Any(y => y.UserId == x.UserId && y.GroupId == x.GroupId).Result);
            await _groupUserRepository.AddRange(toBeAddedGroupUsers, false);

            var concatenatedIds = guList.Select(x => $"{x.GroupId},{x.UserId}");
            await _groupUserRepository.RemoveRange(x => !concatenatedIds.Contains(x.GroupId + "," + x.UserId) && x.Group.RealmId == _realmId);
        }

        private DirectorySearcher CreateDirectorySearcher(DirectoryEntry entry, string filter, string[] propertiesToLoad = null)
        {
            var searcher = new DirectorySearcher(entry)
            {
                Filter = filter,
                SearchScope = SearchScope.Subtree
            };

            if (propertiesToLoad != null)
            {
                foreach (var property in propertiesToLoad)
                {
                    searcher.PropertiesToLoad.Add(property);
                }
            }

            return searcher;
        }
    }
}
