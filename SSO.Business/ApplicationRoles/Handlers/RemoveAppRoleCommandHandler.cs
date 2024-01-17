using MediatR;
using SSO.Business.ApplicationRoles.Commands;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.ApplicationRoles.Handlers
{
    public class RemoveAppRoleCommandHandler : IRequestHandler<RemoveAppRoleCommand, Unit>
    {
        readonly IApplicationRoleRepository _applicationRoleRepository;

        public RemoveAppRoleCommandHandler(IApplicationRoleRepository applicationRoleRepository)
        {
            _applicationRoleRepository = applicationRoleRepository;
        }

        public async Task<Unit> Handle(RemoveAppRoleCommand request, CancellationToken cancellationToken)
        {
            var rec = await _applicationRoleRepository.FindOne(x => x.ApplicationId == request.ApplicationId && x.Id == request.RoleId.ToString());

            if (rec is null)
                throw new ArgumentNullException();

            await _applicationRoleRepository.Delete(rec);

            return new Unit();
        }
    }
}