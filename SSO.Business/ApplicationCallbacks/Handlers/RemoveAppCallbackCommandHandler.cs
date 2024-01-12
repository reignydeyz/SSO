using MediatR;
using SSO.Business.ApplicationCallbacks.Commands;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.ApplicationCallbacks.Handlers
{
    public class RemoveAppCallbackCommandHandler : IRequestHandler<RemoveAppCallbackCommand, Unit>
    {
        readonly IApplicationCallbackRepository _applicationCallbackRepository;

        public RemoveAppCallbackCommandHandler(IApplicationCallbackRepository applicationCallbackRepository)
        {
            _applicationCallbackRepository = applicationCallbackRepository;
        }

        public async Task<Unit> Handle(RemoveAppCallbackCommand request, CancellationToken cancellationToken)
        {
            var rec = await _applicationCallbackRepository.FindOne(x => x.ApplicationId == request.ApplicationId && x.Url == request.Url);

            if (rec is null)
                throw new ArgumentNullException();

            await _applicationCallbackRepository.Delete(rec);

            return new Unit();
        }
    }
}
