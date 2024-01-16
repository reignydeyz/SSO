using AutoMapper;
using MediatR;
using SSO.Business.ApplicationRoles.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;

namespace SSO.Business.ApplicationRoles.Handlers
{
    public class CreateAppRoleCommandHandler : IRequestHandler<CreateAppRoleCommand, AppRoleDto>
    {
        readonly IApplicationRoleRepository _roleRepository;
        readonly IMapper _mapper;

        public CreateAppRoleCommandHandler(IApplicationRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<AppRoleDto> Handle(CreateAppRoleCommand request, CancellationToken cancellationToken)
        {
            var rec = _mapper.Map<ApplicationRole>(request);

            var res = await _roleRepository.Add(rec);

            return _mapper.Map<AppRoleDto>(res);
        }
    }
}
