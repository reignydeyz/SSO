namespace SSO.Domain.Models
{
    public class ApplicationPermission
    {
        public Guid PermissionId { get; set; }
        public Guid ApplicationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Application Application { get; set; }
    }
}
