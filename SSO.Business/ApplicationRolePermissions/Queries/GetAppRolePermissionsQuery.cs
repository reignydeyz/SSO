using MediatR;
using SSO.Business.ApplicationPermissions;
using SSO.Business.ApplicationRoles;

namespace SSO.Business.ApplicationRolePermissions.Queries
{
    public class GetAppRolePermissionsQuery : AppRoleIdDto, IRequest<IEnumerable<AppPermissionDto>>
    {
    }
}
