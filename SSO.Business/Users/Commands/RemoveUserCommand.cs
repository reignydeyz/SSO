using MediatR;

namespace SSO.Business.Users.Commands
{
    public class RemoveUserCommand : IRequest<Unit>
    {
        public string UserId { get; set; }
    }
}
