using MediatR;

namespace SSO.Business.ApplicationUserRoles.Commands
{
    public class UpdateAppUserRolesCommand : AppUserIdDto, IRequest<Unit>
    {
        public IEnumerable<Guid> RoleIds { get; set; } = new List<Guid>();
    }
}
