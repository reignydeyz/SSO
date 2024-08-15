using MediatR;
using SSO.Infrastructure.Settings.Enums;

namespace SSO.Business.RealmIdpSettings.Commands
{
    public class RemoveRealmIdpSettingsCommand : IRequest<Unit>
    {
        public Guid RealmId { get; set; }
        public IdentityProvider IdentityProvider { get; set; }
    }
}
