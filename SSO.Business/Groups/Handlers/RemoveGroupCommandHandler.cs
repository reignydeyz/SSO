using MediatR;
using SSO.Business.Groups.Commands;

namespace SSO.Business.Groups.Handlers
{
    public class RemoveGroupCommandHandler : IRequestHandler<RemoveGroupCommand, Unit>
    {
        readonly RepositoryFactory _groupRepoFactory;

        public RemoveGroupCommandHandler(RepositoryFactory groupRepoFactory)
        {
            _groupRepoFactory = groupRepoFactory;
        }

        public async Task<Unit> Handle(RemoveGroupCommand request, CancellationToken cancellationToken)
        {
            var groupRepo = await _groupRepoFactory.GetRepository(request.RealmId);

            var rec = await groupRepo.FindOne(x => x.GroupId == request.GroupId);

            if (rec is null)
                throw new ArgumentNullException();

            await groupRepo.Delete(rec);

            return new Unit();
        }
    }
}
