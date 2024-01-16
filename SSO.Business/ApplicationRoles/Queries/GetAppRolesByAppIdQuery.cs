using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SSO.Business.ApplicationRoles.Queries
{
    public class GetAppRolesByAppIdQuery : IRequest<IEnumerable<AppRoleDto>>
    {
        [Required]
        public Guid ApplicationId { get; set; }
    }
}
