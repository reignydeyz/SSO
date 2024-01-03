using MediatR;

namespace SSO.Business.Users.Queries
{
    public class GetUsersQuery : IRequest<IQueryable<UserDto>>
    {
    }
}
