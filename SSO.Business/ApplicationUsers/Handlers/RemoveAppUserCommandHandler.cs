using MediatR;
using SSO.Business.ApplicationUsers.Commands;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.ApplicationUsers.Handlers
{
    public class RemoveAppUserCommandHandler : IRequestHandler<RemoveAppUserCommand, Unit>
    {
        readonly IUserRoleRepository _userRoleRepository;

        public RemoveAppUserCommandHandler(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<Unit> Handle(RemoveAppUserCommand request, CancellationToken cancellationToken)
        {
            await _userRoleRepository.RemoveUser(request.UserId, request.ApplicationId);

            return new Unit();
        }
    }
}