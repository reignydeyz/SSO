using MediatR;

namespace SSO.Business.Groups.Commands
{
    public class RemoveGroupCommand : IRequest<Unit>
    {
        public Guid GroupId { get; set; }
    }
}
