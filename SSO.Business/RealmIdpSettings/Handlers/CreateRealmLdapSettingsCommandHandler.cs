using MediatR;
using Newtonsoft.Json;
using SSO.Business.RealmIdpSettings.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Infrastructure.Settings.Enums;

namespace SSO.Business.RealmIdpSettings.Handlers
{
    public class CreateRealmLdapSettingsCommandHandler : IRequestHandler<CreateRealmLdapSettingsCommand, Unit>
    {
        IRealmIdpSettingsRepository _realmIdpSettingsRepository;

        public CreateRealmLdapSettingsCommandHandler(IRealmIdpSettingsRepository realmIdpSettingsRepository)
        {
            _realmIdpSettingsRepository = realmIdpSettingsRepository;
        }

        public async Task<Unit> Handle(CreateRealmLdapSettingsCommand request, CancellationToken cancellationToken)
        {
            var entry = new Domain.Models.RealmIdpSettings
            {
                RealmId = request.RealmId,
                IdentityProvider = IdentityProvider.LDAP,
                Value = JsonConvert.SerializeObject(request.LDAPSettings)
            };

            await _realmIdpSettingsRepository.Add(entry);

            return new Unit();
        }
    }
}
