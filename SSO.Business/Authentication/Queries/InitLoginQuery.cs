using MediatR;
using SSO.Business.Applications;
using System.ComponentModel.DataAnnotations;

namespace SSO.Business.Authentication.Queries
{
    public class InitLoginQuery : ApplicationIdDto, IRequest<Unit>
    {
        [Required, Url(ErrorMessage = "Not a valid Uri.")]
        public string CallbackUrl { get; set; }
    }
}
