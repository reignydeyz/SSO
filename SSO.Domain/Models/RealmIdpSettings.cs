using SSO.Infrastructure.Settings.Enums;

namespace SSO.Domain.Models
{
    public class RealmIdpSettings
    {
        public Guid RealmId { get; set; }
        public IdentityProvider IdentityProvider { get; set; }
        public string Value { get; set; }

        public virtual Realm Realm { get; set; }
    }
}
