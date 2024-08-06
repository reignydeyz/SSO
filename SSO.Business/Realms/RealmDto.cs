using SSO.Business.RealmIdpSettings;

namespace SSO.Business.Realms
{
    public class RealmDto
    {
        public Guid RealmId { get; set; }
        public string Name { get; set; }
        public IEnumerable<RealmIdpSettingsDto> RealmIdpSettingsCollection { get; set; } = new List<RealmIdpSettingsDto>();
    }
}
