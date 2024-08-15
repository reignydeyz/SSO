using MediatR;
using SSO.Business.Users;
using System.Text.Json.Serialization;

namespace SSO.Business.GroupUsers.Queries
{
    public class GetUsersByGroupIdQuery : IRequest<IQueryable<UserDto>>
    {
        [JsonIgnore]
        public Guid RealmId { get; set; }

        public Guid GroupId { get; set; }
    }
}
