using AutoMapper;
using Newtonsoft.Json;
using SSO.Infrastructure.LDAP.Models;
using SSO.Infrastructure.Settings.Enums;
using SSO.Infrastructure.Settings.Services;

namespace SSO.Business.RealmIdpSettings
{
    public class ValueResolver : IValueResolver<Domain.Models.RealmIdpSettings, RealmIdpSettingsDto, object?>
    {
        readonly JwtSecretService _jwtSecretService;
        readonly RsaKeyService _rsaKeyService;

        public ValueResolver(JwtSecretService jwtSecretService, RsaKeyService rsaKeyService)
        {
            _jwtSecretService = jwtSecretService;
            _rsaKeyService = rsaKeyService;
        }

        public object? Resolve(Domain.Models.RealmIdpSettings source, RealmIdpSettingsDto destination, object? destMember, ResolutionContext context)
        {
            if (source.IdentityProvider == IdentityProvider.LDAP)
            {
                var decStr = _rsaKeyService.DecryptString(source.Value, _jwtSecretService.PrivateKey);

                return JsonConvert.DeserializeObject<LDAPSettings>(decStr);
            }

            return null;
        }
    }
}
