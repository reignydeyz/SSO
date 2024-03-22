using MediatR;

namespace SSO.Business.GroupUsers.Commands
{
    public class RemoveGroupUserCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }
    }
}
