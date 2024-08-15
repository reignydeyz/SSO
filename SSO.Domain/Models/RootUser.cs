namespace SSO.Domain.Models
{
    public class RootUser
    {
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
