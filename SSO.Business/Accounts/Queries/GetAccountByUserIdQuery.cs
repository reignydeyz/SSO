using MediatR;

namespace SSO.Business.Accounts.Queries
{
    public class GetAccountByUserIdQuery : IRequest<AccountDto>
    {
        public string UserId { get; set; }
    }
}
