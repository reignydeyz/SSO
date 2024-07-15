using Microsoft.AspNetCore.Identity;

namespace SSO.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = "admin";
        public string LastName { get; set; } = "admin";
        public DateTime? PasswordExpiry { get; set; }
        public string LastSessionId { get; set; } = "35c7c988-7c48-4f13-bf41-4edbd060a394";
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastPasswordChanged { get; set; }
        public DateTime? LastFailedPasswordAttempt { get; set; }
        public DateTime? DateConfirmed { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = "admin";
        public DateTime DateModified { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = "admin";
        public DateTime? DateInactive { get; set; }

        public virtual ICollection<GroupUser> Groups { get; set; } = new List<GroupUser>();
        public virtual ICollection<RealmUser> Realms { get; set; } = new List<RealmUser>();
    }
}
