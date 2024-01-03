using AutoMapper;
using SSO.Business.Users;
using SSO.Domain.Models;

namespace SSO.Business.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(x => x.UserId, from => from.MapFrom(x => x.Id))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
