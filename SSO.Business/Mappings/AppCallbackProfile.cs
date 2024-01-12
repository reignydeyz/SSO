using AutoMapper;
using SSO.Business.ApplicationCallbacks;
using SSO.Business.ApplicationCallbacks.Commands;
using SSO.Domain.Models;

namespace SSO.Business.Mappings
{
    public class AppCallbackProfile : Profile
    {
        public AppCallbackProfile()
        {
            CreateMap<CreateAppCallbackCommand, ApplicationCallback>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<ApplicationCallback, AppCallbackDto>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
