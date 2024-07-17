using MediatR;
using System.Text.Json.Serialization;

namespace SSO.Business.GroupUsers.Commands
{
    public class RemoveGroupUserCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public Guid RealmId { get; set; }

        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }
    }
}
