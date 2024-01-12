using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SSO.Business.ApplicationCallbacks.Commands
{
    public class RemoveAppCallbackCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public Guid ApplicationId { get; set; }

        [Required, Url]
        public string Url { get; set; }
    }
}
