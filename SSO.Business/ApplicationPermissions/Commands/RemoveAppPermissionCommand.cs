using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SSO.Business.ApplicationPermissions.Commands
{
    public class RemoveAppPermissionCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public Guid ApplicationId { get; set; }

        [Required]
        public Guid PermissionId { get; set; }
    }
}
