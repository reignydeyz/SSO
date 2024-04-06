using MediatR;
using SSO.Business.ApplicationGroupRoles.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;

namespace SSO.Business.ApplicationGroupRoles.Handlers
{
    public class UpdateGroupRolesCommandHandler : IRequestHandler<UpdateGroupRolesCommand, Unit>
    {
        readonly IApplicationRoleRepository _roleRepository;
        readonly IGroupRoleRepository _groupRoleRepository;

        public UpdateGroupRolesCommandHandler(IApplicationRoleRepository roleRepository, IGroupRoleRepository groupRoleRepository)
        {
            _roleRepository = roleRepository;
            _groupRoleRepository = groupRoleRepository;
        }

        public async Task<Unit> Handle(UpdateGroupRolesCommand request, CancellationToken cancellationToken)
        {
            var roles = await _roleRepository.Find(x => x.ApplicationId == request.ApplicationId);

            if (!request.RoleIds.Distinct().All(x => roles.Select(y => y.Id).Contains(x.ToString())))
                throw new ArgumentException(message: "Some of the roles are invalid.", paramName: "InvalidRole");

            var toBeDeleted = await _groupRoleRepository.GroupRoles(request.GroupId, request.ApplicationId);
            await _groupRoleRepository.RemoveRange(toBeDeleted, false);

            var selectedRoleIds = request.RoleIds.Select(x => x.ToString());
            var toBeAdded = from r in await _roleRepository.Find(x => selectedRoleIds.Contains(x.Id))
                            select new GroupRole { GroupId = request.GroupId, RoleId = r.Id };
            await _groupRoleRepository.AddRange(toBeAdded);

            return new Unit();
        }
    }
}
