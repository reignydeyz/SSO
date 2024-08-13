using MediatR;
using SSO.Business.RealmIdpSettings.Commands;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.RealmIdpSettings.Handlers
{
    public class RemoveRealmIdpSettingsCommandHandler : IRequestHandler<RemoveRealmIdpSettingsCommand, Unit>
    {
        readonly IRealmIdpSettingsRepository _repository;

        public RemoveRealmIdpSettingsCommandHandler(IRealmIdpSettingsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(RemoveRealmIdpSettingsCommand request, CancellationToken cancellationToken)
        {
            var rec = await _repository.FindOne(x => x.RealmId == request.RealmId && x.IdentityProvider == request.IdentityProvider);

            if (rec is null)
                throw new ArgumentNullException();

            await _repository.Delete(rec);

            return new Unit();
        }
    }
}
