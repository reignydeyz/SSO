using MediatR;
using SSO.Business.Accounts.Commands;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.Accounts.Handlers
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        readonly IUserRepository _userRepository;

        public ChangePasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.User is null)
                throw new ArgumentNullException(message: "User is not selected.", paramName: "NoUser");

            await _userRepository.ChangePassword(request.User, request.NewPassword);

            return new Unit();
        }
    }
}
