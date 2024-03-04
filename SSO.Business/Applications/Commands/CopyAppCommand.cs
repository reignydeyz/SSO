using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SSO.Business.Applications.Commands
{
    public class CopyAppCommand : IRequest<ApplicationDetailDto>
    {
        /// <summary>
        /// Source app
        /// </summary>
        [JsonIgnore]
        public Guid ApplicationId { get; set; }

        [Required, MinLength(3), StringLength(200)]
        public string Name { get; set; }

        [JsonIgnore]
        public string? Author { get; set; }
    }
}
