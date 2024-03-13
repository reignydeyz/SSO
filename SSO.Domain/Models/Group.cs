namespace SSO.Domain.Models
{
    public class Group
    {
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = "admin";
        public DateTime DateModified { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = "admin";
        public DateTime? DateInactive { get; set; }
    }
}
