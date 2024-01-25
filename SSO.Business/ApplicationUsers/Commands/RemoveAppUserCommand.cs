using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SSO.Business.ApplicationUsers.Commands
{
    public class RemoveAppUserCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public Guid ApplicationId { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
