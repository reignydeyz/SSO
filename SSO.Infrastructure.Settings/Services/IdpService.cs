using SSO.Infrastructure.Settings.Enums;

namespace SSO.Infrastructure.Settings.Services
{
    public class IdpService
    {
        public IdentityProvider IdentityProvider { get; }

        public IdpService(IdentityProvider idp) => IdentityProvider = idp;
    }
}
