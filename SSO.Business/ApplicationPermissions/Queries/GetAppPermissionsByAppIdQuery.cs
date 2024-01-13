using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SSO.Business.ApplicationPermissions.Queries
{
    public class GetAppPermissionsByAppIdQuery : IRequest<IEnumerable<AppPermissionDto>>
    {
        [Required]
        public Guid ApplicationId { get; set; }
    }
}
