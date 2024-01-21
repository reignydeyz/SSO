using MediatR;

namespace SSO.Business.Applications.Commands
{
    public class RemoveAppCommand : IRequest<Unit>
    {
        public Guid ApplicationId { get; set; }
    }
}
