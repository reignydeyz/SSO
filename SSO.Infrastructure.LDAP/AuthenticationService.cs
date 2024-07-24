using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Models;
using SSO.Infrastructure.LDAP.Models;
using SSO.Infrastructure.Settings.Enums;
using System.DirectoryServices;

namespace SSO.Infrastructure.LDAP
{
    public class AuthenticationService : IAuthenticationService
    {
        readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Login(string username, string password, Application app)
        {
            if (app.Realm.IdpSettingsCollection == null || !app.Realm.IdpSettingsCollection.Any(x => x.IdentityProvider == IdentityProvider.LDAP))
                throw new ArgumentException("LDAP is not configured.");

            var ldapSettings = JsonConvert.DeserializeObject<LDAPSettings>(app.Realm.IdpSettingsCollection.First(x => x.IdentityProvider == IdentityProvider.LDAP).Value);
            var ldapConnectionString = $"{(ldapSettings.UseSSL ? "LDAPS" : "LDAP")}://{ldapSettings.Server}:{ldapSettings.Port}/{ldapSettings.SearchBase}";

            var user = await _userManager.FindByNameAsync(username);

            if (user is null)
                throw new UnauthorizedAccessException("User not found.");

            if (user.DateInactive != null)
                throw new UnauthorizedAccessException("Account is inactive");

            if (await _userManager.IsLockedOutAsync(user))
                throw new UnauthorizedAccessException("User is locked-out.");

            try
            {
                using (DirectoryEntry entry = new DirectoryEntry(ldapConnectionString, username, password, AuthenticationTypes.Secure))
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
