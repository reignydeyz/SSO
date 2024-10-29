using MediatR;
using SSO.Domain.Models;
using System.Text.Json.Serialization;

namespace SSO.Business.Accounts.Commands
{
    public class Generate2faQrCodeCommand : IRequest<string>
    {
        [JsonIgnore]
        public ApplicationUser? User { get; set; }

        [JsonIgnore]
        public ApplicationUser? Author { get; set; }
    }
}
