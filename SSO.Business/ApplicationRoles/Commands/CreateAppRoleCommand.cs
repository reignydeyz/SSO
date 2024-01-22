using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SSO.Business.ApplicationRoles.Commands
{
    public class CreateAppRoleCommand : IRequest<AppRoleDto>
    {
        [JsonIgnore]
        public Guid ApplicationId { get; set; }

        [Required, MinLength(3), StringLength(100)]
        public string Name { get; set; }
    }
}
