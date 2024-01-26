using MediatR;
using SSO.Business.ApplicationUserRoles.Commands;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.ApplicationUserRoles.Handlers
{
    public class UpdateAppUserRolesCommandHandler : IRequestHandler<UpdateAppUserRolesCommand, Unit>
    {
        readonly IApplicationRoleRepository _applicationRoleRepository;
        readonly IApplicationRoleClaimRepository _applicationRoleClaimRepository;
        readonly IApplicationPermissionRepository _applicationPermissionRepository;
        readonly IUserRoleRepository _userRoleRepository;
        readonly IUserClaimRepository _userClaimRepository;

        public UpdateAppUserRolesCommandHandler(IApplicationRoleRepository applicationRoleRepository, 
            IApplicationPermissionRepository applicationPermissionRepository,
            IApplicationRoleClaimRepository applicationRoleClaimRepository,
            IUserRoleRepository userRoleRepository, IUserClaimRepository userClaimRepository)
        {
            _applicationRoleRepository = applicationRoleRepository;
            _applicationPermissionRepository = applicationPermissionRepository;
            _applicationRoleClaimRepository = applicationRoleClaimRepository;
            _userRoleRepository = userRoleRepository;
            _userClaimRepository = userClaimRepository;
        }

        public async Task<Unit> Handle(UpdateAppUserRolesCommand request, CancellationToken cancellationToken)
        {
            var roles = await _applicationRoleRepository.Find(x => x.ApplicationId == request.ApplicationId);

            if (!request.RoleIds.Distinct().All(x => roles.Select(y => y.Id).Contains(x.ToString())))
                throw new ArgumentException(message: "Some of the roles are invalid.", paramName: "InvalidRole");

            var toBeDeleted = await _userRoleRepository.Roles(request.UserId, request.ApplicationId);
            await _userRoleRepository.RemoveRoles(request.UserId, toBeDeleted, false);

            var permissions = await _applicationPermissionRepository.Find(x => x.ApplicationId == request.ApplicationId);            
            await _userClaimRepository.RemoveClaims(request.UserId, permissions, false);
            
            var selectedRoleIds = request.RoleIds.Select(x => x.ToString());
            var toBeAdded = await _applicationRoleRepository.Find(x => selectedRoleIds.Contains(x.Id));
            await _userRoleRepository.AddRoles(request.UserId, toBeAdded, false);

            var claims = await _applicationRoleClaimRepository.Find(x => selectedRoleIds.Contains(x.RoleId));
            permissions = claims.Select(x => x.Permission).Distinct();
            await _userClaimRepository.AddClaims(request.UserId, permissions);

            return new Unit();
        }
    }
}
