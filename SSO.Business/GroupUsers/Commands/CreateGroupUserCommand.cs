using MediatR;
using System.Text.Json.Serialization;

namespace SSO.Business.GroupUsers.Commands
{
    public class CreateGroupUserCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public Guid RealmId { get; set; }

        [JsonIgnore]
        public Guid GroupId { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}
