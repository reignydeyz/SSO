using MediatR;
using Microsoft.AspNetCore.Identity;
using SSO.Business.Accounts.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;

namespace SSO.Business.Accounts.Handlers
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        readonly UserManager<ApplicationUser> _userManager;
        readonly IUserRepository _userRepository;

        public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.User is null)
                throw new ArgumentNullException(message: "User is not selected.", paramName: "NoUser");

            if (!(await _userManager.CheckPasswordAsync(request.User, request.CurrentPassword)))
                throw new ArgumentException(message: "Incorrect password.", paramName: "InvalidPassword");

            await _userRepository.ChangePassword(request.User, request.NewPassword);

            return new Unit();
        }
    }
}
