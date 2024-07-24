using MediatR;
using SSO.Infrastructure.LDAP.Models;

namespace SSO.Business.RealmIdpSettings.Commands
{
    public class CreateRealmLdapSettingsCommand : IRequest<Unit>
    {
        public Guid RealmId { get; set; }
        public LDAPSettings LDAPSettings { get; set; }
    }
}
