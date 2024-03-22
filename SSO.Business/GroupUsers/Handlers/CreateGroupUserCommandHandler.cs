using MediatR;
using SSO.Business.GroupUsers.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;

namespace SSO.Business.GroupUsers.Handlers
{
    public class CreateGroupUserCommandHandler : IRequestHandler<CreateGroupUserCommand, Unit>
    {
        readonly IGroupUserRepository _groupUserRepository;

        public CreateGroupUserCommandHandler(IGroupUserRepository groupUserRepository)
        {
            _groupUserRepository = groupUserRepository;
        }

        public async Task<Unit> Handle(CreateGroupUserCommand request, CancellationToken cancellationToken)
        {
            if (!(await _groupUserRepository.Any(x => x.UserId == request.UserId.ToString() && x.GroupId == request.GroupId)))
                await _groupUserRepository.Add(new GroupUser { GroupId = request.GroupId, UserId = request.UserId.ToString() });
                    
            return new Unit();
        }
    }
}
