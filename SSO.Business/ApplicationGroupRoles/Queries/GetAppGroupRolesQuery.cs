using MediatR;
using SSO.Business.ApplicationRoles;

namespace SSO.Business.ApplicationGroupRoles.Queries
{
    public class GetAppGroupRolesQuery : AppGroupIdDto, IRequest<IEnumerable<AppRoleDto>>
    {
    }
}
