using MediatR;

namespace SSO.Business.Applications.Queries
{
    public class GetAppByIdQuery : IRequest<ApplicationDto>
    {
        public Guid AppId { get; set; }
    }
}
