using SSO.Infrastructure.Settings.Enums;

namespace SSO.Infrastructure.Settings.Services
{
    public class RealmService
    {
        public Realm Realm { get; }

        public RealmService(Realm realm) => Realm = realm;
    }
}
