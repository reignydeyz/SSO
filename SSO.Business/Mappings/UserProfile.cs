using AutoMapper;
using SSO.Business.Accounts;
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

            CreateMap<ApplicationUser, AccountDto>()
                .ForMember(x => x.UserId, from => from.MapFrom(x => x.Id))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<ApplicationUser, UserDetailDto>()
                .ForMember(x => x.UserId, from => from.MapFrom(x => x.Id))
                .ForMember(x => x.Username, from => from.MapFrom(x => x.UserName))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<CreateUserCommand, ApplicationUser>()
                .ForMember(x => x.Id, from => from.MapFrom(x => Guid.NewGuid().ToString()))
                .ForMember(x => x.UserName, from => from.MapFrom(x => x.Username))
                .ForMember(x => x.PasswordHash, from => from.MapFrom(x => x.Password))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<UpdateUserCommand, ApplicationUser>()
                .ForMember(x => x.Id, from => from.MapFrom(x => x.UserId!))
                .ForMember(x => x.UserName, from => from.MapFrom(x => x.Username))
                .ForMember(x => x.PasswordHash, from => from.MapFrom(x => x.Password))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<AccountDto, UserDetailDto>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter()
                .ReverseMap();
        }
    }
}
