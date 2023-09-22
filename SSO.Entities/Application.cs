namespace SSO.Entities
{
    public class Application
    {
        public Guid ApplicationId { get; set; }
        public string Name { get; set; }
        public string? RedirectUrl { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset DateModified { get; set; }
        public DateTimeOffset? DateInactive { get; set; }
    }
}
