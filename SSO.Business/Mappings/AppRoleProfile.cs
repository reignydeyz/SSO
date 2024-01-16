using AutoMapper;
using SSO.Domain.Models;
using SSO.Business.ApplicationRoles.Commands;
using SSO.Business.ApplicationRoles;

namespace SSO.Business.Mappings
{
    public class AppRoleProfile : Profile
    {
        public AppRoleProfile()
        {
            CreateMap<CreateAppRoleCommand, ApplicationRole>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<ApplicationRole, AppRoleDto>()
                .ForMember(x => x.RoleId, from => from.MapFrom(x => x.Id))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
