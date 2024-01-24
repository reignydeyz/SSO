using MediatR;
using SSO.Business.ApplicationRoles;

namespace SSO.Business.ApplicationUserRoles.Queries
{
    public class GetAppUserRolesQuery : AppUserIdDto, IRequest<IEnumerable<AppRoleDto>>
    {
    }
}
