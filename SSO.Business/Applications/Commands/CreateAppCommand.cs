using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SSO.Business.Applications.Commands
{
    public class CreateAppCommand : IRequest<ApplicationDto>
    {
        [Required, MinLength(6), StringLength(200)]
        public string Name { get; set; }

        [JsonIgnore]
        public string? Author { get; set; }
    }
}
