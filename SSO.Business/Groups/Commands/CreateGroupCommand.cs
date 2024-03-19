using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SSO.Business.Groups.Commands
{
    public class CreateGroupCommand : IRequest<GroupDto>
    {
        [Required, MinLength(3), StringLength(200)]
        public string Name { get; set; }

        [MinLength(3), StringLength(500)]
        public string? Description { get; set; }

        [JsonIgnore]
        public string? Author { get; set; }
    }
}
