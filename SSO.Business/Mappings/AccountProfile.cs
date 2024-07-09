using AutoMapper;
using SSO.Business.Accounts.Commands;

namespace SSO.Business.Mappings
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<ChangePasswordCommand, ChangePasswordLdapCommand>()
                .ReverseMap();
        }
    }
}
