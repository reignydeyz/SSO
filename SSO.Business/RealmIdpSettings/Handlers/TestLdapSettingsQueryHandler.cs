using MediatR;
using SSO.Business.RealmIdpSettings.Queries;
using System.DirectoryServices;

namespace SSO.Business.RealmIdpSettings.Handlers
{
    public class TestLdapSettingsQueryHandler : IRequestHandler<TestLdapSettingsQuery, Unit>
    {
        public async Task<Unit> Handle(TestLdapSettingsQuery request, CancellationToken cancellationToken)
        {
            var ldapConnectionString = $"{(request.UseSSL ? "LDAPS" : "LDAP")}://{request.Server}:{request.Port}/{request.SearchBase}";

            using (DirectoryEntry entry = new DirectoryEntry(ldapConnectionString, request.Username, request.Password, AuthenticationTypes.Secure))
            {
                // Attempt to bind to the directory using the provided credentials
                object nativeObject = entry.NativeObject;

                // If no exception is thrown, the credentials are valid
                Console.WriteLine($"Success!");
            }

            return new Unit();
        }
    }
}
