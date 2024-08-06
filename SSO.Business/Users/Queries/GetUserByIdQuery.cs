using MediatR;
using System.Text.Json.Serialization;

namespace SSO.Business.Users.Queries
{
    public class GetUserByIdQuery : IRequest<UserDetailDto>
    {
        [JsonIgnore]
        public Guid RealmId { get; set; }

        public string UserId { get; set; }
    }
}
