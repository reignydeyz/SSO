using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SSO.Business.Applications.Commands
{
    public class UpdateAppCommand : IRequest<ApplicationDetailDto>
    {
        [JsonIgnore]
        public Guid ApplicationId { get; set; }

        [Required, MinLength(6), StringLength(200)]
        public string Name { get; set; }

        [JsonIgnore]
        public string? Author { get; set; }

        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Value must be greater than or equal to 1.")]
        public int TokenExpiration { get; set; }

        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Value must be greater than or equal to 1.")]
        public int RefreshTokenExpiration { get; set; }

        [Range(minimum: 0, maximum: short.MaxValue, ErrorMessage = "Value must be greater than or equal to 1.")]
        public short MaxAccessFailedCount { get; set; } = 0;
    }
}
