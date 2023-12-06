using Microsoft.AspNetCore.Identity;

namespace SSO.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = "admin";
        public string LastName { get; set; } = "admin";
    }
}
