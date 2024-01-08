using MediatR;

namespace SSO.Business.Applications.Queries
{
    public class GetAppByIdQuery : IRequest<ApplicationDetailDto>
    {
        public Guid AppId { get; set; }
    }
}
