using Microsoft.AspNetCore.Identity;

namespace SSO.Domain.Models
{
    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        public Guid PermissionId { get; set; }

        public ApplicationPermission Permission { get; set; }
    }
}
