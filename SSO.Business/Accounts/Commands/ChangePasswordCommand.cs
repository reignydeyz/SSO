using MediatR;
using SSO.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SSO.Business.Accounts.Commands
{
    public class ChangePasswordCommand : IRequest<Unit>
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [StringLength(100, MinimumLength = 7, ErrorMessage = "The password must be between {2} and {1} characters long.")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{7,}$",
                   ErrorMessage = "The password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character.")]
        public string NewPassword { get; set; }

        [Required, Compare("NewPassword")]
        public string RepeatPassword { get; set; }

        [JsonIgnore]
        public ApplicationUser? User { get; set; }

        [JsonIgnore]
        public ApplicationUser? Author { get; set; }

        [JsonIgnore]
        public Guid? RealmId { get; set; }
    }
}
