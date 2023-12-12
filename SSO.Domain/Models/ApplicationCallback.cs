namespace SSO.Domain.Models
{
    public class ApplicationCallback
    {
        public Guid ApplicationId { get; set; }

        public string Url { get; set; }

        public virtual Application Application { get; set; }
    }
}
