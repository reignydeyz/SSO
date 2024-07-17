using MediatR;
using SSO.Business.Users.Commands;

namespace SSO.Business.Users.Handlers
{
    public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, Unit>
    {
        readonly RepositoryFactory _repoFactory;

        public RemoveUserCommandHandler(RepositoryFactory repoFactory)
        {
            _repoFactory = repoFactory;
        }

        public async Task<Unit> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            var userRepo = await _repoFactory.GetRepository(request.RealmId);

            var rec = await userRepo.FindOne(x => x.Id == request.UserId);

            if (rec is null)
                throw new ArgumentNullException();

            await userRepo.Delete(rec);

            return new Unit();
        }
    }
}
