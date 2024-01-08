using System.ComponentModel.DataAnnotations;

namespace SSO.Business.Applications
{
    public class ApplicationIdDto
    {
        [Required]
        public Guid? ApplicationId { get; set; }
    }
}
