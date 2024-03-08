using MediatR;
using SSO.Business.ApplicationRolePermissions.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;

namespace SSO.Business.ApplicationRolePermissions.Handlers
{
    public class UpdateAppRolePermissionsCommandHandler : IRequestHandler<UpdateAppRolePermissionsCommand, Unit>
    {
        readonly IApplicationPermissionRepository _applicationPermissionRepository;
        readonly IApplicationRoleRepository _applicationRoleRepository;
        readonly IApplicationRoleClaimRepository _applicationRoleClaimRepository;

        public UpdateAppRolePermissionsCommandHandler(IApplicationPermissionRepository applicationPermissionRepository, 
            IApplicationRoleRepository applicationRoleRepository,
            IApplicationRoleClaimRepository applicationRoleClaimRepository)
        {
            _applicationPermissionRepository = applicationPermissionRepository;
            _applicationRoleRepository = applicationRoleRepository;
            _applicationRoleClaimRepository = applicationRoleClaimRepository;
        }

        public async Task<Unit> Handle(UpdateAppRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            var permissions = await _applicationPermissionRepository.Find(x => x.ApplicationId == request.ApplicationId);

            if (!request.PermissionIds.Distinct().All(x => permissions.Select(y => y.PermissionId).Contains(x)))
                throw new ArgumentException(message: "Some of the permissions are invalid.", paramName: "InvalidPermission");

            var toBeDeleted = await _applicationRoleClaimRepository.Find(x => x.RoleId == request.RoleId.ToString());
            await _applicationRoleClaimRepository.RemoveRange(toBeDeleted, false);

            var users = await _applicationRoleRepository.GetUsers(request.RoleId);

            permissions = permissions.Where(x => request.PermissionIds.Contains(x.PermissionId));
            var toBeAdded = permissions
                .Select(x => new ApplicationRoleClaim {
                    PermissionId = x.PermissionId,
                    RoleId = request.RoleId.ToString(),
                    ClaimType = "Permission",
                    ClaimValue = x.Name
                });
            await _applicationRoleClaimRepository.AddRange(toBeAdded);

            return new Unit();
        }
    }
}
