using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SSO.Business.Authentication.Queries
{
    public class LoginToSystemQuery : IRequest<TokenDto>
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [JsonIgnore]
        public Guid RealmId { get; set; }
    }
}
