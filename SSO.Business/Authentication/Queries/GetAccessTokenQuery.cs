using MediatR;

namespace SSO.Business.Authentication.Queries
{
    public class GetAccessTokenQuery : IRequest<TokenDto>
    {
        public Guid RequestToken { get; set; }
    }
}
