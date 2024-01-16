using MediatR;
using SSO.Business.ApplicationPermissions.Commands;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.ApplicationPermissions.Handlers
{
    public class RemoveAppPermissionCommandHandler : IRequestHandler<RemoveAppPermissionCommand, Unit>
    {
        readonly IApplicationPermissionRepository _applicationPermissionRepository;

        public RemoveAppPermissionCommandHandler(IApplicationPermissionRepository applicationPermissionRepository)
        {
            _applicationPermissionRepository = applicationPermissionRepository;
        }

        public async Task<Unit> Handle(RemoveAppPermissionCommand request, CancellationToken cancellationToken)
        {
            var rec = await _applicationPermissionRepository.FindOne(x => x.ApplicationId == request.ApplicationId && x.PermissionId == request.PermissionId);

            if (rec is null)
                throw new ArgumentNullException();

            await _applicationPermissionRepository.Delete(rec);

            return new Unit();
        }
    }
}
