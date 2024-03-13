namespace SSO.Domain.Models
{
    public class GroupUser
    {
        public Guid GroupId { get; set; }
        public string UserId { get; set; }

        public virtual Group Group { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
