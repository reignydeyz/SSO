using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SSO.Business.ApplicationGroups.Commands
{
    public class RemoveAppGroupCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public Guid ApplicationId { get; set; }

        [Required]
        public Guid GroupId { get; set; }
    }
}
