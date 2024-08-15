using AutoMapper;
using SSO.Business.RealmIdpSettings.Queries;
using SSO.Infrastructure.LDAP.Models;

namespace SSO.Business.Mappings
{
    public class RealmIdpSettingsProfile : Profile
    {
        public RealmIdpSettingsProfile()
        {
            CreateMap<LDAPSettings, TestLdapSettingsQuery>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ReverseMap();
        }
    }
}
