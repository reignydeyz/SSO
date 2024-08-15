using AutoMapper;
using Newtonsoft.Json;
using SSO.Business.RealmIdpSettings;
using SSO.Business.Realms;
using SSO.Domain.Models;
using SSO.Infrastructure.LDAP.Models;
using SSO.Infrastructure.Settings.Enums;

namespace SSO.Business.Mappings
{
    public class RealmProfile : Profile
    {
        public RealmProfile()
        {
            CreateMap<Realm, RealmDto>()
                .ForMember(x => x.RealmIdpSettingsCollection, from => from.MapFrom(x => x.IdpSettingsCollection))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<Domain.Models.RealmIdpSettings, RealmIdpSettingsDto>()
                .ForMember(x => x.Value, from => from.MapFrom(x => x.IdentityProvider == IdentityProvider.LDAP
                    ? JsonConvert.DeserializeObject<LDAPSettings>(x.Value)
                    : null)
                )
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
