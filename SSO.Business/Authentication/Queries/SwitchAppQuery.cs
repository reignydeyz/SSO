using MediatR;
using SSO.Business.Applications;

namespace SSO.Business.Authentication.Queries
{
    public class SwitchAppQuery : ApplicationIdDto, IRequest<TokenDto>
    {
        public string Token { get; set; }
    }
}
