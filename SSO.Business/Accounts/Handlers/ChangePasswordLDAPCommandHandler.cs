using MediatR;
using Microsoft.Extensions.Options;
using SSO.Business.Accounts.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Infrastructure.LDAP.Models;
using System.DirectoryServices;

namespace SSO.Business.Accounts.Handlers
{
    public class ChangePasswordLdapCommandHandler : IRequestHandler<ChangePasswordLdapCommand, Unit>
    {
        readonly IUserRepository _userRepository;
        readonly string _ldapConnectionString;

        public ChangePasswordLdapCommandHandler(IUserRepository userRepository, IOptions<LDAPSettings> ldapSettings)
        {
            _ldapConnectionString = $"{(ldapSettings.Value.UseSSL ? "LDAPS" : "LDAP")}://{ldapSettings.Value.Server}:{ldapSettings.Value.Port}/{ldapSettings.Value.SearchBase}";
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(ChangePasswordLdapCommand request, CancellationToken cancellationToken)
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
