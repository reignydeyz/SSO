using AutoMapper;
using SSO.Business.Applications;
using SSO.Business.Applications.Commands;
using SSO.Domain.Models;

namespace SSO.Business.Mappings
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Application, ApplicationDto>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();

            CreateMap<Application, ApplicationDetailDto>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();

            CreateMap<CreateAppCommand, Application>()
                .ForMember(x => x.TokenExpiration, from => from.MapFrom(x => 1440))
                .ForMember(x => x.RefreshTokenExpiration, from => from.MapFrom(x => 10080))
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();

            CreateMap<CopyAppCommand, Application>()
                .ForMember(x => x.ApplicationId, from => from.MapFrom(x => Guid.NewGuid()))
                .ForMember(x => x.TokenExpiration, from => from.MapFrom(x => 1440))
                .ForMember(x => x.RefreshTokenExpiration, from => from.MapFrom(x => 10080))
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();

            CreateMap<UpdateAppCommand, Application>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
        }
    }
}
