using MediatR;

namespace SSO.Business.ApplicationGroupRoles.Commands
{
    public class UpdateGroupRolesCommand : IRequest<Unit>
    {
        public Guid ApplicationId { get; set; }
        public Guid GroupId { get; set; }
        public IEnumerable<Guid> RoleIds { get; set; } = new List<Guid>();
    }
}
