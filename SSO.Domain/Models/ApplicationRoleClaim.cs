using Microsoft.AspNetCore.Identity;

namespace SSO.Domain.Models
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public Guid PermissionId { get; set; }

        public ApplicationPermission Permission { get; set; }
    }
}
