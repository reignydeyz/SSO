using MediatR;

namespace SSO.Business.Applications.Queries
{
    public class GetApplicationsQuery : IRequest<IQueryable<ApplicationDto>>
    {
        public Guid RealmId { get; set; }
    }
}
