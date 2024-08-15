using MediatR;
using Newtonsoft.Json;
using SSO.Business.RealmIdpSettings.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Infrastructure.Settings.Enums;

namespace SSO.Business.RealmIdpSettings.Handlers
{
    public class ModifyRealmLdapSettingsCommandHandler : IRequestHandler<ModifyRealmLdapSettingsCommand, Unit>
    {
        IRealmIdpSettingsRepository _realmIdpSettingsRepository;

        public ModifyRealmLdapSettingsCommandHandler(IRealmIdpSettingsRepository realmIdpSettingsRepository)
        {
            _realmIdpSettingsRepository = realmIdpSettingsRepository;
        }

        public async Task<Unit> Handle(ModifyRealmLdapSettingsCommand request, CancellationToken cancellationToken)
        {
            var rec = await _realmIdpSettingsRepository.FindOne(x => x.RealmId == request.RealmId && x.IdentityProvider == IdentityProvider.LDAP);

            if (rec is not null)
                await _realmIdpSettingsRepository.Delete(rec, false);

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
