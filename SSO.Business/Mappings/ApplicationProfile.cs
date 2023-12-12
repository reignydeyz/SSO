using AutoMapper;
using SSO.Business.Applications;
using SSO.Domain.Models;

namespace SSO.Business.Mappings
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Application, ApplicationDto>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
        }
    }
}
