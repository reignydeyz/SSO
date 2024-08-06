using MediatR;
using System.Text.Json.Serialization;

namespace SSO.Business.Groups.Queries
{
    public class GetGroupsQuery : IRequest<IQueryable<GroupDto>>
    {
        [JsonIgnore]
        public Guid RealmId { get; set; }
    }
}
