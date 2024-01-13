using MediatR;
using SSO.Business.ApplicationCallbacks.Queries;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.ApplicationCallbacks.Handlers
{
    public class GetAppCallbacksByAppIdQueryHandler : IRequestHandler<GetAppCallbacksByAppIdQuery, IEnumerable<string>>
    {
        readonly IApplicationRepository _applicationRepository;

        public GetAppCallbacksByAppIdQueryHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public async Task<IEnumerable<string>> Handle(GetAppCallbacksByAppIdQuery request, CancellationToken cancellationToken)
        {
            var res = await _applicationRepository.GetCallbacks(request.ApplicationId);

            return res.Select(x => x.Url);
        }
    }
}
