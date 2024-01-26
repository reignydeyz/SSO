using MediatR;

namespace SSO.Business.Users.Queries
{
    public class GetUserByIdQuery : IRequest<UserDetailDto>
    {
        public string UserId { get; set; }
    }
}
