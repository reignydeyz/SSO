using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SSO.Business.Authentication.Queries
{
    public class LoginAsRootQuery : IRequest<TokenDto>
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
