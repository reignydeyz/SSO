using MediatR;
using SSO.Business.Applications;

namespace SSO.Business.UserApplications.Queries
{
    public class GetUserAppsQuery : IRequest<IEnumerable<ApplicationDto>>
    {
        public Guid UserId { get; set; }
    }
}
