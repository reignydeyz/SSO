using MediatR;
using SSO.Business.Users;

namespace SSO.Business.GroupUsers.Queries
{
    public class GetUsersByGroupIdQuery : IRequest<IQueryable<UserDto>>
    {
        public Guid GroupId { get; set; }
    }
}
