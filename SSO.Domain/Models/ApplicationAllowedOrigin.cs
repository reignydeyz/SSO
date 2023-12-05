namespace SSO.Domain.Models
{
    public class ApplicationAllowedOrigin
    {
        public Guid ApplicationId { get; set; }
        public string Origin { get; set; }
    }
}
