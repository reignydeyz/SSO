using MediatR;
using SSO.Domain.Models;
using System.Text.Json.Serialization;

namespace SSO.Business.Accounts.Commands
{
    public class Disable2faCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public ApplicationUser? User { get; set; }

        [JsonIgnore]
        public ApplicationUser? Author { get; set; }
    }
}
