using MediatR;
using SSO.Business.Users.Commands;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.Users.Handlers
{
    public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, Unit>
    {
        readonly IUserRepository _userRepository;

        public RemoveUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            var rec = await _userRepository.FindOne(x => x.Id == request.UserId);

            if (rec is null)
                throw new ArgumentNullException();

            await _userRepository.Delete(rec);

            return new Unit();
        }
    }
}
