using MediatR;
using System.Text.Json.Serialization;

namespace SSO.Business.Groups.Commands
{
    public class RemoveGroupCommand : IRequest<Unit>
    {
        public Guid GroupId { get; set; }

        [JsonIgnore]
        public Guid RealmId { get; set; }
    }
}
