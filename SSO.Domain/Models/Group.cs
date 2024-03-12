namespace SSO.Domain.Models
{
    public class Group
    {
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset DateModified { get; set; }
        public DateTimeOffset? DateInactive { get; set; }
    }
}
