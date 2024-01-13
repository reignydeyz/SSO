using AutoMapper;
using SSO.Business.ApplicationPermissions;
using SSO.Domain.Models;

namespace SSO.Business.Mappings
{
    public class AppPermissionProfile : Profile
    {
        public AppPermissionProfile()
        {
            CreateMap<ApplicationPermission, AppPermissionDto>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
