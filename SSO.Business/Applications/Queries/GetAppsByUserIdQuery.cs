using MediatR;
using SSO.Business.Applications;
using System.ComponentModel.DataAnnotations;

namespace SSO.Business.ApplicationUsers.Queries
{
    public class GetAppsByUserIdQuery : IRequest<IEnumerable<ApplicationDto>>
    {
        [Required]
        public Guid UserId { get; set; }
    }
}
