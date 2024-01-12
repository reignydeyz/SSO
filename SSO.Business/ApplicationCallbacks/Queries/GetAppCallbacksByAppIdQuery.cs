using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SSO.Business.ApplicationCallbacks.Queries
{
    public class GetAppCallbacksByAppIdQuery : IRequest<IEnumerable<string>>
    {
        [Required]
        public Guid ApplicationId { get; set; }
    }
}
