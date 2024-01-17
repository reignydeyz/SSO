using AutoMapper;
using SSO.Business.ApplicationPermissions;
using SSO.Domain.Models;

namespace SSO.Business.Mappings
{
    public class ApplicationRoleClaimProfile : Profile
    {
        public ApplicationRoleClaimProfile()
        {
            CreateMap<ApplicationRoleClaim, AppPermissionDto>()
                .ForMember(x => x.Name, from => from.MapFrom(x => x.Permission.Name))
                .ForMember(x => x.Description, from => from.MapFrom(x => x.Permission.Description))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
