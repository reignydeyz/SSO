using MediatR;
using SSO.Business.Users.Commands;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.Users.Handlers
{
    public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, Unit>
    {
        readonly IRealmRepository _realmRepository;
        readonly RepositoryFactory _repoFactory;

        public RemoveUserCommandHandler(IRealmRepository realmRepository, RepositoryFactory repoFactory)
        {
            _realmRepository = realmRepository;
            _repoFactory = repoFactory;
        }

        public async Task<Unit> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            var realm = await _realmRepository.FindOne(x => x.RealmId == request.RealmId);
            var userRepo = await _repoFactory.GetRepository(request.RealmId);

            var rec = await userRepo.FindOne(x => x.Id == request.UserId);

            if (rec is null)
                throw new ArgumentNullException();

            await userRepo.Delete(rec, true, realm);

            return new Unit();
        }
    }
}
