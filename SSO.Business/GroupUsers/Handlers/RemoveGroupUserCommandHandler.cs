using MediatR;
using SSO.Business.GroupUsers.Commands;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.GroupUsers.Handlers
{
    public class RemoveGroupUserCommandHandler : IRequestHandler<RemoveGroupUserCommand, Unit>
    {
        readonly IGroupUserRepository _groupUserRepository;

        public RemoveGroupUserCommandHandler(IGroupUserRepository groupUserRepository)
        {
            _groupUserRepository = groupUserRepository;
        }

        public async Task<Unit> Handle(RemoveGroupUserCommand request, CancellationToken cancellationToken)
        {
            var rec = await _groupUserRepository.FindOne(x => x.GroupId == request.GroupId && x.UserId == request.UserId.ToString());

            if (rec is null)
                throw new ArgumentNullException();

            await _groupUserRepository.Delete(rec!);

            return new Unit();
        }
    }
}
