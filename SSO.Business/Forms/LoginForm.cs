using System.ComponentModel.DataAnnotations;

namespace SSO.Business.Forms
{
    public class LoginForm
    {
        public Guid? ApplicationId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
