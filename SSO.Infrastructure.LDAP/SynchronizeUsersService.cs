using Microsoft.Extensions.Options;
using SSO.Domain.Models;
using SSO.Infrastructure.LDAP.Models;
using System.DirectoryServices;

namespace SSO.Infrastructure.LDAP
{
    public class SynchronizeUsersService
    {
        readonly AppDbContext _context;
        readonly LDAPSettings _ldapSettings;
        readonly string _ldapConnectionString;

        public SynchronizeUsersService(IOptions<LDAPSettings> ldapSettings, AppDbContext context)
        {
            _ldapSettings = ldapSettings.Value;
            _ldapConnectionString = $"{(_ldapSettings.UseSSL ? "LDAPS" : "LDAP")}://{_ldapSettings.Server}:{_ldapSettings.Port}/{_ldapSettings.SearchBase}";
            _context = context;
        }

        public async Task Begin()
        {
            using (DirectoryEntry entry = new DirectoryEntry(_ldapConnectionString, _ldapSettings.Username, _ldapSettings.Password))
            {                
                await ProcessGroups(entry);

                await ProcessUsers(entry);

                await ProcessGroupUsers(entry);
            }
        }

        private async Task ProcessGroups(DirectoryEntry entry)
        {
            using (var searcher = new DirectorySearcher(entry))
            {
                searcher.Filter = "(&(objectClass=group)(objectCategory=group))";
                searcher.ReferralChasing = ReferralChasingOption.All;

                SearchResultCollection groupResults = searcher.FindAll();

                var groups = groupResults.Cast<SearchResult>()
                                    .Where(x => x.Properties["sAMAccountName"]?.Count > 0)
                                    .Select(x => new Group
                                    {
                                        GroupId = Guid.NewGuid(),
                                        Name = x.Properties["sAMAccountName"][0].ToString(),
                                        Description = x.Properties["description"]?.Count > 0
                                            ? x.Properties["description"][0].ToString()
                                            : null
                                    }).ToList();

                var toBeAdded = groups.Where(x => !_context.Groups.Any(y => y.Name.ToLower() == x.Name.ToLower()));
                await _context.AddRangeAsync(toBeAdded);

                var groupNames = groups.Select(x => x.Name.ToLower());
                var toBeDeleted = _context.Groups
                    .Where(x => !groupNames.Any(y => y == x.Name.ToLower()))
                    .ToList();
                _context.RemoveRange(toBeDeleted);

                _context.SaveChanges();
            }
        }

        private async Task ProcessUsers(DirectoryEntry entry)
        {
            using (var searcher = new DirectorySearcher(entry))
            {
                searcher.Filter = _ldapSettings.SearchFilter;

                // Handle referrals explicitly
                searcher.ReferralChasing = ReferralChasingOption.All;

                // Perform the search
                SearchResultCollection results = searcher.FindAll();

                var users = new List<ApplicationUser>();

                foreach (SearchResult result in results)
                {
                    if (result.Properties["sAMAccountName"]?.Count > 0 &&
                        result.Properties["givenName"]?.Count > 0 &&
                        result.Properties["sn"]?.Count > 0)
                    {
                        var newUser = new ApplicationUser
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserName = result.Properties["sAMAccountName"][0].ToString(),
                            NormalizedUserName = result.Properties["sAMAccountName"][0].ToString().ToUpper(),
                            FirstName = result.Properties["givenName"][0].ToString(),
                            LastName = result.Properties["sn"][0].ToString(),
                            PasswordHash = Guid.NewGuid().ToString()
                        };

                        users.Add(newUser);
                    }
                }

                var toBeAdded = users.Where(x => !_context.Users.Any(y => y.UserName.ToLower() == x.UserName.ToLower() || (y.FirstName.ToLower() == x.FirstName.ToLower() && y.LastName.ToLower() == x.LastName.ToLower())));
                await _context.AddRangeAsync(toBeAdded);

                var usernames = users.Select(x => x.UserName.ToLower());
                var toBeDeleted = _context.Users
                    .Where(u => !usernames.Any(x => x == u.UserName.ToLower()))
                    .ToList();
                _context.RemoveRange(toBeDeleted);

                await _context.SaveChangesAsync();
            }
        }

        private async Task ProcessGroupUsers(DirectoryEntry entry)
        {
            using (var searcher = new DirectorySearcher(entry))
            {
                searcher.Filter = _ldapSettings.SearchFilter;

                // Handle referrals explicitly
                searcher.ReferralChasing = ReferralChasingOption.All;

                // Perform the search
                SearchResultCollection results = searcher.FindAll();

                var users = new List<ApplicationUser>();

                foreach (SearchResult result in results)
                {
                    if (result.Properties["sAMAccountName"]?.Count > 0 &&
                        result.Properties["givenName"]?.Count > 0 &&
                        result.Properties["sn"]?.Count > 0)
                    {
                        // Get the user's groups directly from DirectoryEntry
                        using (DirectoryEntry userEntry = result.GetDirectoryEntry())
                        {
                            var username = result.Properties["sAMAccountName"][0].ToString();
                            var user = _context.Users.First(x => x.UserName == username);

                            // Clear first
                            _context.GroupUsers.RemoveRange(_context.GroupUsers.Where(x => x.UserId == user.Id));

                            // Check if the user has the "memberOf" attribute
                            if (userEntry.Properties.Contains("memberOf"))
                            {
                                foreach (string groupPath in userEntry.Properties["memberOf"])
                                {
                                    // Extract the group name from the LDAP path
                                    string groupName = groupPath.Split(',')[0].Substring(3);
                                    
                                    var group = _context.Groups.First(x => x.Name == groupName);

                                    _context.GroupUsers.Add(new GroupUser { GroupId = group.GroupId, UserId = user.Id });
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
