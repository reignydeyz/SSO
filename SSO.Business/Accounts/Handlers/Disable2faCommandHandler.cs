using MediatR;
using Microsoft.AspNetCore.Identity;
using SSO.Business.Accounts.Commands;
using SSO.Domain.Models;

namespace SSO.Business.Accounts.Handlers
{
    public class Disable2faCommandHandler : IRequestHandler<Disable2faCommand, Unit>
    {
        readonly UserManager<ApplicationUser> _userManager;

        public Disable2faCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(Disable2faCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.User!.Id.ToString());

            user.TwoFactorEnabled = false;
            user.TwoFactorSecretKeyHash = null;

            await _userManager.UpdateAsync(user);

            return new Unit();
        }
    }
}
