using MediatR;
using SSO.Business.Applications.Commands;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.Applications.Handlers
{
    public class RemoveAppCommandHandler : IRequestHandler<RemoveAppCommand, Unit>
    {
        readonly IApplicationRepository _applicationRepository;

        public RemoveAppCommandHandler(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public async Task<Unit> Handle(RemoveAppCommand request, CancellationToken cancellationToken)
        {
            var rec = await _applicationRepository.FindOne(x => x.ApplicationId == request.ApplicationId);

            if (rec is null)
                throw new ArgumentNullException();

            await _applicationRepository.Delete(rec);

            return new Unit();
        }
    }
}
