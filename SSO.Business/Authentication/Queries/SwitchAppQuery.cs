using MediatR;

namespace SSO.Business.Authentication.Queries
{
    public class SwitchAppQuery : AuthDto, IRequest<TokenDto>
    {
        public string Token { get; set; }
    }
}
