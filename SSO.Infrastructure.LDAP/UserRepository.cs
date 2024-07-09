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
        private readonly IAppDbContext _context;
        private readonly DirectoryEntry _dirEntry;
        private readonly DirectorySearcher _dirSearcher;
        private readonly UserManager<ApplicationUser> _userManager;
        private bool _disposed;

        public UserRepository(UserManager<ApplicationUser> userManager, IAppDbContext context, IOptions<LDAPSettings> ldapSettings) : base(userManager, context)
        {
            _context = context;

            var ldapConnectionString = $"{(ldapSettings.Value.UseSSL ? "LDAPS" : "LDAP")}://{ldapSettings.Value.Server}:{ldapSettings.Value.Port}/{ldapSettings.Value.SearchBase}";
            _dirEntry = new DirectoryEntry(ldapConnectionString, ldapSettings.Value.Username, ldapSettings.Value.Password);
            _dirSearcher = new DirectorySearcher(_dirEntry);
            _userManager = userManager;
        }

        public override async Task<ApplicationUser> Add(ApplicationUser param, bool? saveChanges = true, object? args = null)
        {
            CreateUserInLDAP(param);

            await _context.AddAsync(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();

            return param;
        }

        public override async Task AddRange(IEnumerable<ApplicationUser> param, bool? saveChanges = true, object? args = null)
        {
            _context.AddRange(param);
            await SaveChangesIfNeeded(saveChanges);
        }

        public override async Task ChangePassword(ApplicationUser applicationUser, string password, ApplicationUser? author = null)
        {
            var userEntry = FindUserEntry(applicationUser.UserName);
            SetPasswordAndUpdateEntry(userEntry, password);

            await UpdatePasswordInIdentity(applicationUser, password, author);
        }

        public override async Task Delete(ApplicationUser param, bool? saveChanges = true)
        {
            var userEntry = FindUserEntry(param.UserName);

            if (userEntry != null)
            {
                DeleteUserEntry(userEntry);

                _context.Remove(param);

                if (saveChanges!.Value)
                    await _context.SaveChangesAsync();
            }
        }

        public override async Task RemoveRange(IEnumerable<ApplicationUser> param, bool? saveChanges = true, object? args = null)
        {
            _context.RemoveRange(param);
            await SaveChangesIfNeeded(saveChanges);
        }

        public override async Task RemoveRange(Expression<Func<ApplicationUser, bool>> predicate, bool? saveChanges = true, object? args = null)
        {
            _context.RemoveRange(_context.ApplicationUsers.Where(predicate));
            await SaveChangesIfNeeded(saveChanges);
        }

        public override async Task<ApplicationUser> Update(ApplicationUser param, bool? saveChanges = true, object? args = null)
        {
            var userEntry = FindUserEntry(param.UserName);
            RenameAndUpdateUserEntry(userEntry, param);

            await ManageUserSecurity(param);

            return param;
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

        private void CreateUserInLDAP(ApplicationUser param)
        {
            var newUser = _dirEntry.Children.Add($"CN={param.UserName}", "user");
            newUser.Properties["samAccountName"].Value = param.UserName;
            newUser.Properties["userPassword"].Value = param.PasswordHash;
            newUser.Properties["givenName"].Value = param.FirstName;
            newUser.Properties["sn"].Value = param.LastName;
            newUser.CommitChanges();

            newUser.Invoke("SetPassword", new object[] { param.PasswordHash });
            newUser.CommitChanges();

            int val = (int)newUser.Properties["userAccountControl"].Value;
            newUser.Properties["userAccountControl"].Value = val & ~(0x2); // Enable account flag
            newUser.CommitChanges();

            param.NormalizedUserName = param.UserName.ToUpper();
        }

        private DirectoryEntry FindUserEntry(string userName)
        {
            _dirSearcher.Filter = $"(samAccountName={userName})";
            _dirSearcher.SearchScope = SearchScope.Subtree;

            var result = _dirSearcher.FindOne();
            return result?.GetDirectoryEntry();
        }

        private void SetPasswordAndUpdateEntry(DirectoryEntry userEntry, string password)
        {
            userEntry.Invoke("SetPassword", new object[] { password });
            userEntry.Properties["LockOutTime"].Value = 0;
            userEntry.CommitChanges();
        }

        private async Task UpdatePasswordInIdentity(ApplicationUser applicationUser, string password, ApplicationUser? author)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);
            var result = await _userManager.ResetPasswordAsync(applicationUser, token, password);

            if (!result.Succeeded && result.Errors.Any())
                throw new ArgumentException(result.Errors.First().Description);

            var rec = _context.ApplicationUsers.First(x => x.Id == applicationUser.Id);
            rec.DateModified = DateTime.Now;
            rec.ModifiedBy = author == null ? $"{applicationUser.FirstName} {applicationUser.LastName}" : $"{author.FirstName} {author.LastName}";

            await _context.SaveChangesAsync();
        }

        private void DeleteUserEntry(DirectoryEntry userEntry)
        {
            if (userEntry != null)
            {
                userEntry.DeleteTree();
                _dirEntry.CommitChanges();
            }
        }

        private async Task SaveChangesIfNeeded(bool? saveChanges)
        {
            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        private void RenameAndUpdateUserEntry(DirectoryEntry userEntry, ApplicationUser param)
        {
            if (userEntry != null)
            {
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

            _context.SaveChanges();
        }

        private async Task ManageUserSecurity(ApplicationUser user)
        {
            var userEntry = FindUserEntry(user.UserName);

            if (userEntry != null)
            {
                // Unlock user account in LDAP
                userEntry.Properties["LockOutTime"].Value = 0;
                userEntry.CommitChanges();
            }

            // Reset access failed count and lockout end date in Identity system
            await _userManager.ResetAccessFailedCountAsync(user);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MinValue);
        }
    }
}
