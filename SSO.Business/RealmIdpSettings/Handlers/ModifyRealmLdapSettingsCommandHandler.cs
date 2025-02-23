using MediatR;
using Newtonsoft.Json;
using SSO.Business.RealmIdpSettings.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Infrastructure.Settings.Enums;
using SSO.Infrastructure.Settings.Services;
using System.Security.Cryptography;

namespace SSO.Business.RealmIdpSettings.Handlers
{
    public class ModifyRealmLdapSettingsCommandHandler : IRequestHandler<ModifyRealmLdapSettingsCommand, Unit>
    {
        readonly IRealmIdpSettingsRepository _realmIdpSettingsRepository;
        readonly JwtSecretService _jwtSecretService;
        readonly RsaKeyService _rsaKeyService;

        public ModifyRealmLdapSettingsCommandHandler(IRealmIdpSettingsRepository realmIdpSettingsRepository, JwtSecretService jwtSecretService, RsaKeyService rsaKeyService)
        {
            _realmIdpSettingsRepository = realmIdpSettingsRepository;
            _jwtSecretService = jwtSecretService;
            _rsaKeyService = rsaKeyService;
        }

        public async Task<Unit> Handle(ModifyRealmLdapSettingsCommand request, CancellationToken cancellationToken)
        {
            var rec = await _realmIdpSettingsRepository.FindOne(x => x.RealmId == request.RealmId && x.IdentityProvider == IdentityProvider.LDAP);

            if (rec is not null)
                await _realmIdpSettingsRepository.Delete(rec, false);

            var publicKey = RSA.Create(); publicKey.ImportParameters(_jwtSecretService.PrivateKey.ExportParameters(false));
            var secret = _rsaKeyService.EncryptString(JsonConvert.SerializeObject(request.LDAPSettings), publicKey);

            var entry = new Domain.Models.RealmIdpSettings
            {
                RealmId = request.RealmId,
                IdentityProvider = IdentityProvider.LDAP,
                Value = secret
            };

            await _realmIdpSettingsRepository.Add(entry);

            return new Unit();
        }
    }
}
