namespace SSO.Domain.Models
{
    public class GroupRole
    {
        public Guid GroupId { get; set; }
        public string RoleId { get; set; }
        public virtual Group Group { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}
