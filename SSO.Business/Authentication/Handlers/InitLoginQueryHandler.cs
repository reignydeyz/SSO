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
            var callbackUrls = await _applicationRepository.GetCallbackUrls(request.AppId!.Value);

            if (!callbackUrls.Contains(request.CallbackUrl))
                throw new ArgumentException(message: "Callback url is invalid.", paramName: "InvalidCallback");

            return new();
        }
    }
}
