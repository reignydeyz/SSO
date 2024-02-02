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
            _ldapConnectionString = $"LDAP://{_ldapSettings.Server}:{_ldapSettings.Port}/{_ldapSettings.SearchBase}";
            _context = context;
        }

        public async Task Begin()
        {
            using (DirectoryEntry root = new DirectoryEntry(_ldapConnectionString, _ldapSettings.Username, _ldapSettings.Password, AuthenticationTypes.Secure))
            {
                using (var searcher = new DirectorySearcher(root))
                {
                    searcher.Filter = "(&(objectClass=user)(objectCategory=person))";

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
                            users.Add(new ApplicationUser
                            {
                                UserName = result.Properties["sAMAccountName"][0].ToString(),
                                NormalizedUserName = result.Properties["sAMAccountName"][0].ToString().ToUpper(),
                                FirstName = result.Properties["givenName"][0].ToString(),
                                LastName = result.Properties["sn"][0].ToString(),
                                PasswordHash = Guid.NewGuid().ToString(),
                            });
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
        }
    }
}
