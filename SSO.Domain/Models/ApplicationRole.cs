using Microsoft.AspNetCore.Identity;

namespace SSO.Domain.Models
{
    public class ApplicationRole : IdentityRole
    {
        public Guid ApplicationId { get; set; }

        public Application Application { get; set; }
    }
}
