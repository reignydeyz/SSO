using MediatR;
using System.Text.Json.Serialization;

namespace SSO.Business.Users.Queries
{
    public class GetUsersQuery : IRequest<IQueryable<UserDto>>
    {
        [JsonIgnore]
        public Guid RealmId { get; set; }
    }
}
