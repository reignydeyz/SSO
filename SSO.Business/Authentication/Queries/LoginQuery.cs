using MediatR;
using SSO.Business.Applications;
using System.ComponentModel.DataAnnotations;

namespace SSO.Business.Authentication.Queries
{
    public class LoginQuery : ApplicationIdDto, IRequest<TokenDto>
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
