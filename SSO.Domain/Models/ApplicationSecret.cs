namespace SSO.Domain.Models
{
    public class ApplicationSecret
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string SecretHash { get; set; }
        public string SecretSalt { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
    }
}
