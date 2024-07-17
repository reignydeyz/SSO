using MediatR;
using Microsoft.AspNetCore.Identity;
using SSO.Business.Accounts.Commands;
using SSO.Domain.Models;

namespace SSO.Business.Accounts.Handlers
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        readonly UserManager<ApplicationUser> _userManager;
        readonly Users.RepositoryFactory _userRepoFactory;

        public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager, Users.RepositoryFactory userRepoFactory)
        {
            _userManager = userManager;
            _userRepoFactory = userRepoFactory;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var userRepo = await _userRepoFactory.GetRepository();

            if (request.User is null)
                throw new ArgumentNullException(message: "User is not selected.", paramName: "NoUser");

            if (!(await _userManager.CheckPasswordAsync(request.User, request.CurrentPassword)))
                throw new ArgumentException(message: "Incorrect password.", paramName: "InvalidPassword");

            await userRepo.ChangePassword(request.User, request.NewPassword);

            return new Unit();
        }
    }
}
