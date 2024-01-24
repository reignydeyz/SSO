using MediatR;
using SSO.Business.ApplicationUserRoles.Commands;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.ApplicationUserRoles.Handlers
{
    public class UpdateAppUserRolesCommandHandler : IRequestHandler<UpdateAppUserRolesCommand, Unit>
    {
        readonly IApplicationRoleRepository _applicationRoleRepository;
        readonly IUserRoleRepository _userRoleRepository;

        public UpdateAppUserRolesCommandHandler(IApplicationRoleRepository applicationRoleRepository, IUserRoleRepository userRoleRepository)
        {
            _applicationRoleRepository = applicationRoleRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<Unit> Handle(UpdateAppUserRolesCommand request, CancellationToken cancellationToken)
        {
            var roles = await _applicationRoleRepository.Find(x => x.ApplicationId == request.ApplicationId);

            if (!request.RoleIds.Distinct().All(x => roles.Select(y => y.Id).Contains(x.ToString())))
                throw new ArgumentException(message: "Some of the roles are invalid.", paramName: "InvalidRole");

            var toBeDeleted = await _userRoleRepository.Roles(request.UserId, request.ApplicationId);

            await _userRoleRepository.RemoveRoles(request.UserId, toBeDeleted, false);

            // TODO: Handle user claims

            var selectedRoleIds = request.RoleIds.Select(x => x.ToString());

            var toBeAdded = await _applicationRoleRepository.Find(x => selectedRoleIds.Contains(x.Id));

            await _userRoleRepository.AddRoles(request.UserId, toBeAdded);

            return new Unit();
        }
    }
}
