using MediatR;
using SSO.Business.Users.Commands;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.Users.Handlers
{
    public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, Unit>
    {
        readonly IRealmRepository _realmRepository;
        readonly IRealmUserRepository _realmUserRepository;
        readonly RepositoryFactory _repoFactory;

        public RemoveUserCommandHandler(IRealmRepository realmRepository, IRealmUserRepository realmUserRepository, RepositoryFactory repoFactory)
        {
            _realmRepository = realmRepository;
            _realmUserRepository = realmUserRepository;
            _repoFactory = repoFactory;
        }

        public async Task<Unit> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            var realm = await _realmRepository.FindOne(x => x.RealmId == request.RealmId);

            var realmUser = await _realmUserRepository.FindOne(x => x.RealmId == realm.RealmId && x.UserId == request.UserId);
            await _realmUserRepository.Delete(realmUser);

            var userRepo = await _repoFactory.GetRepository(request.RealmId);

            var rec = await userRepo.FindOne(x => x.Id == request.UserId);

            if (rec is null)
                throw new ArgumentNullException();

            await userRepo.Delete(rec, true, realm);

            return new Unit();
        }
    }
}
