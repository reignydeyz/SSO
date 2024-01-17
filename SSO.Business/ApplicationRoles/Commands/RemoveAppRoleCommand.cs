using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SSO.Business.ApplicationRoles.Commands
{
    public class RemoveAppRoleCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public Guid ApplicationId { get; set; }

        [Required]
        public Guid RoleId { get; set; }
    }
}
