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

        [Required, MinLength(7)]
        public string NewPassword { get; set; }

        [Required, Compare("NewPassword")]
        public string RepeatPassword { get; set; }

        [JsonIgnore]
        public ApplicationUser? User { get; set; }

        [JsonIgnore]
        public ApplicationUser? Author { get; set; }
    }
}
