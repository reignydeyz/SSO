using MediatR;
using SSO.Business.Groups;

namespace SSO.Business.UserGroups.Queries
{
    public class GetUserGroupsQuery : IRequest<IEnumerable<GroupDto>>
    {
        public Guid UserId { get; set; }
    }
}
