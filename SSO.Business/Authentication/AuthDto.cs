using System.ComponentModel.DataAnnotations;

namespace SSO.Business.Authentication
{
    public class AuthDto
    {
        [Required]
        public Guid? AppId { get; set; }
    }
}
