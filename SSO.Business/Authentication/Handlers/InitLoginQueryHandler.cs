using MediatR;
using SSO.Business.Authentication.Queries;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.Authentication.Handlers
{
    public class InitLoginQueryHandler : IRequestHandler<InitLoginQuery, Unit>
    {
        readonly IApplicationRepository _applicationRepository;

        public InitLoginQueryHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public async Task<Unit> Handle(InitLoginQuery request, CancellationToken cancellationToken)
        {
            var callbackUrls = (await _applicationRepository.GetCallbacks(request.ApplicationId!.Value)).Select(x => x.Url);

            if (!callbackUrls.Contains(request.CallbackUrl))
                throw new ArgumentException(message: "Callback url is invalid.", paramName: "InvalidCallback");

            return new();
        }
    }
}
