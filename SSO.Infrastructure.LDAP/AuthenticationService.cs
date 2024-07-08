using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Models;
using SSO.Infrastructure.LDAP.Models;
using System.DirectoryServices;

namespace SSO.Infrastructure.LDAP
{
    public class AuthenticationService : IAuthenticationService
    {
        readonly UserManager<ApplicationUser> _userManager;
        readonly LDAPSettings _ldapSettings;
        readonly string _ldapConnectionString;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IOptions<LDAPSettings> ldapSettings)
        {
            _userManager = userManager;
            _ldapSettings = ldapSettings.Value;
            _ldapConnectionString = $"{(_ldapSettings.UseSSL ? "LDAPS" : "LDAP")}://{_ldapSettings.Server}:{_ldapSettings.Port}/{_ldapSettings.SearchBase}";
        }

        public async Task Login(string username, string password, Application app)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user is null)
                throw new UnauthorizedAccessException("User not found.");

            if (user.DateInactive != null)
                throw new UnauthorizedAccessException("Account is inactive");

            if (await _userManager.IsLockedOutAsync(user))
                throw new UnauthorizedAccessException("User is locked-out.");

            try
            {
                using (DirectoryEntry entry = new DirectoryEntry(_ldapConnectionString, username, password, AuthenticationTypes.Secure))
                {
                    // Attempt to bind to the directory using the provided credentials
                    object nativeObject = entry.NativeObject;

                    // If no exception is thrown, the credentials are valid
                    Console.WriteLine($"Success!");

                    user.LastSessionId = Guid.NewGuid().ToString();
                    user.LastLoginDate = DateTime.Now;
                    await _userManager.UpdateAsync(user);

                    await _userManager.ResetAccessFailedCountAsync(user);
                }
            }
            catch (Exception ex)
            {
                if (app.MaxAccessFailedCount > 0 && app.MaxAccessFailedCount == user.AccessFailedCount)
                {
                    user.LockoutEnabled = true;
                    user.LockoutEnd = DateTime.Now.AddMinutes(30);
                }

                await _userManager.AccessFailedAsync(user);

                user.LastFailedPasswordAttempt = DateTime.Now;

                await _userManager.UpdateAsync(user);

                throw new UnauthorizedAccessException();
            }
        }
    }
}
