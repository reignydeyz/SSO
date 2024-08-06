using MediatR;
using SSO.Business.GroupUsers.Commands;

namespace SSO.Business.GroupUsers.Handlers
{
    public class RemoveGroupUserCommandHandler : IRequestHandler<RemoveGroupUserCommand, Unit>
    {
        readonly RepositoryFactory _groupUserRepoFactory;

        public RemoveGroupUserCommandHandler(RepositoryFactory groupUserRepoFactory)
        {
            _groupUserRepoFactory = groupUserRepoFactory;
        }

        public async Task<Unit> Handle(RemoveGroupUserCommand request, CancellationToken cancellationToken)
        {
            var repo = await _groupUserRepoFactory.GetRepository(request.RealmId);

            var rec = await repo.FindOne(x => x.GroupId == request.GroupId && x.UserId == request.UserId.ToString());

            if (rec is null)
                throw new ArgumentNullException();

            await repo.Delete(rec!);

            return new Unit();
        }
    }
}
