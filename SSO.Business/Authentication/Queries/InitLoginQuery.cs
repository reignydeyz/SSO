using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SSO.Business.Authentication.Queries
{
    public class InitLoginQuery : AuthDto, IRequest<Unit>
    {
        [Required, Url(ErrorMessage = "Not a valid Uri.")]
        public string CallbackUrl { get; set; }
    }
}
