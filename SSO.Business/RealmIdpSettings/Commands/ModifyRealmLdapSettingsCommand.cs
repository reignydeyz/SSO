using MediatR;
using SSO.Infrastructure.LDAP.Models;

namespace SSO.Business.RealmIdpSettings.Commands
{
    public class ModifyRealmLdapSettingsCommand : IRequest<Unit>
    {
        public Guid RealmId { get; set; }
        public LDAPSettings LDAPSettings { get; set; }
    }
}
