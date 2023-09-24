namespace SSO.Domain.Models
{
    public class ApplicationPermission
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string Permission { get; set; }
        public string Description { get; set; }

        public virtual Application Application { get; set; }
    }
}
