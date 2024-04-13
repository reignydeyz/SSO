using MediatR;
using SSO.Business.Groups;

namespace SSO.Business.ApplicationGroups.Queries
{
    public class GetGroupsByApplicationIdQuery : IRequest<IQueryable<GroupDto>>
    {
        public Guid ApplicationId { get; set; }
    }
}
