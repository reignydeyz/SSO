namespace SSO.Domain.Models
{
    public class Application
    {
        public Guid ApplicationId { get; set; }
        public string Name { get; set; }
        public int TokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public short MaxAccessFailedCount { get; set; } = 0;
        public string CreatedBy { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset DateModified { get; set; }
        public DateTimeOffset? DateInactive { get; set; }

        public virtual ICollection<ApplicationPermission> Permissions { get; set; } = new List<ApplicationPermission>();
        public virtual ICollection<ApplicationCallback> Callbacks { get; set; } = new List<ApplicationCallback>();
    }
}
