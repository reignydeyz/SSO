using AutoMapper;
using SSO.Business.ApplicationPermissions;
using SSO.Business.ApplicationPermissions.Commands;
using SSO.Domain.Models;

namespace SSO.Business.Mappings
{
    public class AppPermissionProfile : Profile
    {
        public AppPermissionProfile()
        {
            CreateMap<CreateAppPermissionCommand, ApplicationPermission>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<ApplicationPermission, AppPermissionDto>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
