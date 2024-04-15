using MediatR;
using SSO.Business.Applications;

namespace SSO.Business.GroupApplications.Queries
{
    public class GetGroupAppsQuery : IRequest<IEnumerable<ApplicationDto>>
    {
        public Guid GroupId { get; set; }
    }
}
