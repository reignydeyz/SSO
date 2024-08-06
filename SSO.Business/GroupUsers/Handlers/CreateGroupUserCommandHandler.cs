using MediatR;
using SSO.Business.GroupUsers.Commands;
using SSO.Domain.Models;

namespace SSO.Business.GroupUsers.Handlers
{
    public class CreateGroupUserCommandHandler : IRequestHandler<CreateGroupUserCommand, Unit>
    {
        readonly RepositoryFactory _groupUserRepoFactory;

        public CreateGroupUserCommandHandler(RepositoryFactory groupUserRepoFactory)
        {
            _groupUserRepoFactory = groupUserRepoFactory;
        }

        public async Task<Unit> Handle(CreateGroupUserCommand request, CancellationToken cancellationToken)
        {
            var repo = await _groupUserRepoFactory.GetRepository(request.RealmId);

            if (!(await repo.Any(x => x.UserId == request.UserId.ToString() && x.GroupId == request.GroupId)))
                await repo.Add(new GroupUser { GroupId = request.GroupId, UserId = request.UserId.ToString() });
                    
            return new Unit();
        }
    }
}
