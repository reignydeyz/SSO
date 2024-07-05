using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SSO.Domain.Interfaces;
using SSO.Domain.Models;
using SSO.Infrastructure.LDAP.Models;
using SSO.Infrastructure.Management;
using System.DirectoryServices;
using System.Linq.Expressions;

namespace SSO.Infrastructure.LDAP
{
    public class UserRepository : UserRepositoryBase
    {
        readonly IAppDbContext _context;
        readonly LDAPSettings _ldapSettings;
        readonly string _ldapConnectionString;

        public UserRepository(UserManager<ApplicationUser> userManager,
            IAppDbContext context,
            IOptions<LDAPSettings> ldapSettings) : base(userManager, context)
        {
            _context = context;
            _ldapSettings = ldapSettings.Value;
            _ldapConnectionString = $"{(_ldapSettings.UseSSL ? "LDAPS" : "LDAP")}://{_ldapSettings.Server}:{_ldapSettings.Port}/{_ldapSettings.SearchBase}";
        }

        public override async Task<ApplicationUser> Add(ApplicationUser param, bool? saveChanges = true, object? args = null)
        {
            var entry = new DirectoryEntry(_ldapConnectionString, _ldapSettings.Username, _ldapSettings.Password);
            var newUser = entry.Children.Add($"CN={param.UserName}", "user");
            newUser.Properties["samAccountName"].Value = param.UserName;
            newUser.Properties["userPassword"].Value = param.PasswordHash;
            newUser.Properties["givenName"].Value = param.FirstName;
            newUser.Properties["sn"].Value = param.LastName;
            newUser.CommitChanges();

            // Set the password
            newUser.Invoke("SetPassword", new object[] { param.PasswordHash });
            newUser.CommitChanges();

            // Enable the account by setting userAccountControl attribute
            int val = (int)newUser.Properties["userAccountControl"].Value;
            newUser.Properties["userAccountControl"].Value = val & ~(0x2); // Enable account flag
            newUser.CommitChanges();

            param.NormalizedUserName = param.UserName.ToUpper();
            _context.Add(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();

            return param;
        }

        public override async Task AddRange(IEnumerable<ApplicationUser> param, bool? saveChanges = true, object? args = null)
        {
            _context.AddRange(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public override Task ChangePassword(ApplicationUser applicationUser, string password, ApplicationUser? author = null)
        {
            throw new NotImplementedException();
        }

        public override Task Delete(ApplicationUser param, bool? saveChanges = true)
        {
            throw new NotImplementedException();
        }

        public override async Task RemoveRange(IEnumerable<ApplicationUser> param, bool? saveChanges = true, object? args = null)
        {
            _context.RemoveRange(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public override async Task RemoveRange(Expression<Func<ApplicationUser, bool>> predicate, bool? saveChanges = true, object? args = null)
        {
            _context.RemoveRange(_context.ApplicationUsers.Where(predicate));

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public override Task<ApplicationUser> Update(ApplicationUser param, bool? saveChanges = true, object? args = null)
        {
            throw new NotImplementedException();
        }
    }
}
