using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SSO.Business.Accounts.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using SSO.Infrastructure.LDAP.Models;
using System.DirectoryServices;

namespace SSO.Business.Accounts.Handlers
{
    public class ChangePasswordLDAPCommandHandler : IRequestHandler<ChangePasswordLDAPCommand, Unit>
    {
        readonly LDAPSettings _ldapSettings;
        readonly UserManager<ApplicationUser> _userManager;
        readonly IUserRepository _userRepository;
        readonly string _ldapConnectionString;

        public ChangePasswordLDAPCommandHandler(UserManager<ApplicationUser> userManager, IUserRepository userRepository, IOptions<LDAPSettings> ldapSettings)
        {
            _ldapSettings = ldapSettings.Value;
            _ldapConnectionString = $"{(_ldapSettings.UseSSL ? "LDAPS" : "LDAP")}://{_ldapSettings.Server}:{_ldapSettings.Port}/{_ldapSettings.SearchBase}";
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(ChangePasswordLDAPCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using (DirectoryEntry entry = new DirectoryEntry(_ldapConnectionString, request.User.UserName, request.CurrentPassword, AuthenticationTypes.Secure))
                {
                    // Attempt to bind to the directory using the provided credentials
                    object nativeObject = entry.NativeObject;

                    // If no exception is thrown, the credentials are valid
                    Console.WriteLine($"Success!");
                }

                await _userRepository.ChangePassword(request.User, request.NewPassword);

                return new Unit();
            }
            catch (Exception)
            {
                throw new ArgumentException(message: "Incorrect password.", paramName: "InvalidPassword");
            }
        }
    }
}
