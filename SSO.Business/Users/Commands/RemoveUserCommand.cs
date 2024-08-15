using MediatR;
using System.Text.Json.Serialization;

namespace SSO.Business.Users.Commands
{
    public class RemoveUserCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public Guid RealmId { get; set; }

        public string UserId { get; set; }
    }
}
