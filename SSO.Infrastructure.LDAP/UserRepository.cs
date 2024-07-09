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
    public class UserRepository : UserRepositoryBase, IDisposable
    {
        readonly IAppDbContext _context;
        readonly DirectoryEntry _dirEntry;
        readonly DirectorySearcher _dirSearcher;
        readonly UserManager<ApplicationUser> _userManager;

        bool _disposed;

        public UserRepository(UserManager<ApplicationUser> userManager,
            IAppDbContext context,
            IOptions<LDAPSettings> ldapSettings) : base(userManager, context)
        {
            _context = context;

            var ldapConnectionString = $"{(ldapSettings.Value.UseSSL ? "LDAPS" : "LDAP")}://{ldapSettings.Value.Server}:{ldapSettings.Value.Port}/{ldapSettings.Value.SearchBase}";
            _dirEntry = new DirectoryEntry(ldapConnectionString, ldapSettings.Value.Username, ldapSettings.Value.Password);
            _dirSearcher = new DirectorySearcher(_dirEntry);
            _userManager = userManager;
        }

        public override async Task<ApplicationUser> Add(ApplicationUser param, bool? saveChanges = true, object? args = null)
        {
            var newUser = _dirEntry.Children.Add($"CN={param.UserName}", "user");
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

        public override async Task ChangePassword(ApplicationUser applicationUser, string password, ApplicationUser? author = null)
        {
            _dirSearcher.Filter = $"(samAccountName={applicationUser.UserName})";
            _dirSearcher.SearchScope = SearchScope.Subtree;

            var result = _dirSearcher.FindOne();

            if (result != null)
            {
                var userEntry = result.GetDirectoryEntry();

                userEntry.Invoke("SetPassword", new object[] { password });
                userEntry.Properties["LockOutTime"].Value = 0; // Unlock account if locked
                userEntry.CommitChanges();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);

            var res = await _userManager.ResetPasswordAsync(applicationUser, token, password);

            if (!res.Succeeded && res.Errors.Any())
                throw new ArgumentException(res.Errors.First().Description);

            var rec = _context.ApplicationUsers.First(x => x.Id == applicationUser.Id);

            rec.DateModified = DateTime.Now;
            rec.ModifiedBy = author == null ? $"{applicationUser.FirstName} {applicationUser.LastName}" : $"{author.FirstName} {author.LastName}";

            _context.SaveChanges();
        }

        public override async Task Delete(ApplicationUser param, bool? saveChanges = true)
        {
            _dirSearcher.Filter = $"(samAccountName={param.UserName})";
            _dirSearcher.SearchScope = SearchScope.Subtree;

            var result = _dirSearcher.FindOne();

            if (result != null)
            {
                var userEntry = result.GetDirectoryEntry();
                userEntry.DeleteTree();
                _dirEntry.CommitChanges();
            }

            _context.Remove(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
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

        public override async Task<ApplicationUser> Update(ApplicationUser param, bool? saveChanges = true, object? args = null)
        {
            _dirSearcher.Filter = $"(samAccountName={param.UserName})";
            _dirSearcher.SearchScope = SearchScope.Subtree;

            var result = _dirSearcher.FindOne();

            if (result != null)
            {
                var userEntry = result.GetDirectoryEntry();

                userEntry.Rename($"CN={param.FirstName} {param.LastName}");
                userEntry.Properties["samAccountName"].Value = param.UserName;
                userEntry.Properties["givenName"].Value = param.FirstName;
                userEntry.Properties["sn"].Value = param.LastName;
                userEntry.CommitChanges();
            }

            var rec = _context.ApplicationUsers.First(x => x.Id == param.Id);
            rec.FirstName = param.FirstName;
            rec.LastName = param.LastName;
            rec.UserName = param.UserName;
            rec.NormalizedUserName = param.UserName.ToUpper();
            rec.Email = param.Email;

            await _context.SaveChangesAsync();

            if (param.PasswordHash is not null)
                await ChangePassword(rec, param.PasswordHash, default);

            // Unlock the user
            await _userManager.ResetAccessFailedCountAsync(rec);
            await _userManager.SetLockoutEndDateAsync(rec, DateTimeOffset.MinValue);

            return rec;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dirSearcher?.Dispose();
                    _dirEntry?.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
