using MediatR;
using SSO.Business.Users;

namespace SSO.Business.ApplicationUsers.Queries
{
    public class GetUsersByApplicationIdQuery : IRequest<IQueryable<UserDto>>
    {
        public Guid ApplicationId { get; set; }
    }
}
