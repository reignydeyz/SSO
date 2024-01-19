using AutoMapper;
using SSO.Business.Users;
using SSO.Business.Users.Commands;
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

            CreateMap<CreateUserCommand, ApplicationUser>()
                .ForMember(x => x.Id, from => from.MapFrom(x => Guid.NewGuid().ToString()))
                .ForMember(x => x.PasswordHash, from => from.MapFrom(x => x.Password))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
