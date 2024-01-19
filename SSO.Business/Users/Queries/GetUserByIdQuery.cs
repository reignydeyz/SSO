using MediatR;

namespace SSO.Business.Users.Queries
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public string UserId { get; set; }
    }
}
