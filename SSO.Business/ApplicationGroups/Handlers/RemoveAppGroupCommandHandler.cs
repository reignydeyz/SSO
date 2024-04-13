using MediatR;
using SSO.Business.ApplicationGroups.Commands;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.ApplicationGroups.Handlers
{
    public class RemoveAppGroupCommandHandler : IRequestHandler<RemoveAppGroupCommand, Unit>
    {
        readonly IGroupRoleRepository _groupRoleRepository;

        public RemoveAppGroupCommandHandler(IGroupRoleRepository groupRoleRepository)
        {
            _groupRoleRepository = groupRoleRepository;
        }

        public async Task<Unit> Handle(RemoveAppGroupCommand request, CancellationToken cancellationToken)
        {
            await _groupRoleRepository.RemoveGroup(request.GroupId, request.ApplicationId);

            return new Unit();
        }
    }
}
