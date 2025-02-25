using AutoMapper;
using SSO.Business.RealmIdpSettings;
using SSO.Business.Realms;
using SSO.Domain.Models;

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
                .ForMember(x => x.Value, from => from.MapFrom<ValueResolver>())
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
