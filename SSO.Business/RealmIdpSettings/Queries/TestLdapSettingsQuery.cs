using MediatR;
using SSO.Infrastructure.LDAP.Models;

namespace SSO.Business.RealmIdpSettings.Queries
{
    public class TestLdapSettingsQuery : LDAPSettings, IRequest<Unit>
    {
    }
}
