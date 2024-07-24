using SSO.Infrastructure.Settings.Enums;

namespace SSO.Business.RealmIdpSettings
{
    public class RealmIdpSettingsDto
    {
        public IdentityProvider IdentityProvider { get; set; }

        public object? Value { get; set; }
    }
}
