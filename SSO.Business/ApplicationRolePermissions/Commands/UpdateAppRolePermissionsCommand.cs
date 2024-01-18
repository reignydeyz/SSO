using MediatR;
using SSO.Business.ApplicationRoles;

namespace SSO.Business.ApplicationRolePermissions.Commands
{
    public class UpdateAppRolePermissionsCommand : AppRoleIdDto, IRequest<Unit>
    {
        public IEnumerable<Guid> PermissionIds { get; set; } = new List<Guid>();
    }
}