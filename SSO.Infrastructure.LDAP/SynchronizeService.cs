using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using SSO.Infrastructure.LDAP.Models;
using System.DirectoryServices;

namespace SSO.Infrastructure.LDAP
{
    public class SynchronizeService
    {
        readonly IGroupRepository _groupRepository;
        readonly IGroupUserRepository _groupUserRepository;
        readonly IUserRepository _userRepository;
        readonly LDAPSettings _ldapSettings;
        readonly string _ldapConnectionString;

        public SynchronizeService(IGroupRepository groupRepository,
            IGroupUserRepository groupUserRepository,
            IUserRepository userRepository,
            IOptions<LDAPSettings> ldapSettings)
        {
            _groupRepository = groupRepository;
            _groupUserRepository = groupUserRepository;
            _userRepository = userRepository;
            _ldapSettings = ldapSettings.Value;
            _ldapConnectionString = $"{(_ldapSettings.UseSSL ? "LDAPS" : "LDAP")}://{_ldapSettings.Server}:{_ldapSettings.Port}/{_ldapSettings.SearchBase}";
        }

        public async Task Begin()
        {
            var users = new List<ApplicationUser>();
            var groups = new List<Group>();
            var groupUsers = new List<Tuple<string, string>>();

            using (DirectoryEntry entry = new DirectoryEntry(_ldapConnectionString, _ldapSettings.Username, _ldapSettings.Password))
            {
                using (var searcher = new DirectorySearcher(entry))
                {
                    searcher.Filter = "(objectClass=group)"; // Filter to get only groups
                    searcher.PropertiesToLoad.Add("cn");
                    searcher.PropertiesToLoad.Add("sAMAccountName");
                    searcher.PropertiesToLoad.Add("description");
                    searcher.PropertiesToLoad.Add("distinguishedName");

                    searcher.SearchScope = SearchScope.Subtree;

                    SearchResultCollection results = searcher.FindAll();

                    foreach (SearchResult searchResult in results)
                    {
                        var rec = searchResult.GetDirectoryEntry();
                        groups.Add(new Group { GroupId = Guid.NewGuid(), Name = rec.Properties["cn"].Value?.ToString() ?? string.Empty });
                    }

                    // Groups
                    var toBeAddedGroups = groups.Where(x => !_groupRepository.Any(y => y.Name == x.Name).Result);
                    await _groupRepository.AddRange(toBeAddedGroups, false);

                    var groupNames = groups.Select(x => x.Name.ToLower());
                    await _groupRepository.RemoveRange(x => !groupNames.Contains(x.Name)); // Write
                }

                using (var searcher = new DirectorySearcher(entry))
                {
                    searcher.Filter = _ldapSettings.SearchFilter;

                    // Handle referrals explicitly
                    searcher.ReferralChasing = ReferralChasingOption.All;

                    // Perform the search
                    SearchResultCollection results = searcher.FindAll();

                    foreach (SearchResult result in results)
                    {
                        if (result.Properties["sAMAccountName"]?.Count > 0 &&
                            result.Properties["givenName"]?.Count > 0 &&
                            result.Properties["sn"]?.Count > 0)
                        {
                            users.Add(new ApplicationUser
                            {
                                UserName = result.Properties["sAMAccountName"][0].ToString(),
                                NormalizedUserName = result.Properties["sAMAccountName"][0].ToString().ToUpper(),
                                FirstName = result.Properties["givenName"][0].ToString(),
                                LastName = result.Properties["sn"][0].ToString(),
                                PasswordHash = Guid.NewGuid().ToString()
                            });

                            // Get the user's groups directly from DirectoryEntry
                            using (DirectoryEntry userEntry = result.GetDirectoryEntry())
                            {
                                // Check if the user has the "memberOf" attribute
                                if (userEntry.Properties.Contains("memberOf"))
                                {
                                    foreach (string groupPath in userEntry.Properties["memberOf"])
                                    {
                                        // Extract the group name from the LDAP path
                                        string groupName = groupPath.Split(',')[0].Substring(3);

                                        if (!groupUsers.Any(x => x.Item1 == groupName && x.Item2 == result.Properties["sAMAccountName"][0].ToString()))
                                            groupUsers.Add(new Tuple<string, string>(groupName, result.Properties["sAMAccountName"][0].ToString()));
                                    }
                                }
                            }
                        }
                    }
                }

                // Users
                var toBeAddedUsers = users.Where(x => !_userRepository.Any(y => y.UserName == x.UserName).Result);
                await _userRepository.AddRange(toBeAddedUsers, false);

                var usernames = users.Select(x => x.UserName.ToLower());
                await _userRepository.RemoveRange(x => !usernames.Contains(x.UserName), false);               

                // GroupUsers
                var guList = (from gu in groupUsers
                             join u in await _userRepository.Find(default) on gu.Item2 equals u.UserName
                             join g in await _groupRepository.Find(default) on gu.Item1 equals g.Name
                             select new GroupUser
                             {
                                 GroupId = g.GroupId,
                                 UserId = u.Id
                             });
                var toBeAddedGroupUsers = guList.Where(x => !_groupUserRepository.Any(y => y.UserId == x.UserId && y.GroupId == x.GroupId).Result);
                await _groupUserRepository.AddRange(toBeAddedGroupUsers, false);

                var concatenatedIds = guList.Select(x => $"{x.GroupId},{x.UserId}"); // Because Expression<Func<T, bool>> doesn't support .Any(). Need to find a way to compare
                await _groupUserRepository.RemoveRange(x => !concatenatedIds.Contains(x.GroupId + "," + x.UserId)); // Write
            }
        }
    }
}
