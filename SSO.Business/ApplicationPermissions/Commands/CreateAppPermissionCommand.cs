using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SSO.Business.ApplicationPermissions.Commands
{
    public class CreateAppPermissionCommand : IRequest<AppPermissionDto>
    {
        [JsonIgnore]
        public Guid ApplicationId { get; set; }

        [Required, MinLength(3), StringLength(100)]
        public string Name { get; set; }

        [Required, MinLength(3), StringLength(200)]
        public string Description { get; set; }
    }
}
