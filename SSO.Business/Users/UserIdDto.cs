using System.ComponentModel.DataAnnotations;

namespace SSO.Business.Users
{
    public class UserIdDto
    {
        [Required]
        public Guid? UserId { get; set; }
    }
}
