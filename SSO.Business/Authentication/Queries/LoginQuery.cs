using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SSO.Business.Authentication.Queries
{
    public class LoginQuery : AuthDto, IRequest<TokenDto>
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
