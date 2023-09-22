using Microsoft.AspNetCore.Identity;

namespace SSO.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public Guid ApplicationId { get; set; }

        public Application Application { get; set; }
    }
}
