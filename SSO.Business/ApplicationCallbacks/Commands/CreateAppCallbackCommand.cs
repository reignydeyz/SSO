using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SSO.Business.ApplicationCallbacks.Commands
{
    public class CreateAppCallbackCommand : IRequest<AppCallbackDto>
    {
        [JsonIgnore]
        public Guid ApplicationId { get; set; }

        [Required, Url]
        public string Url { get; set; }
    }
}
