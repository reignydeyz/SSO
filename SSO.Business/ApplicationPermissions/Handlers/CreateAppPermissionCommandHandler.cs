using AutoMapper;
using MediatR;
using SSO.Business.ApplicationPermissions.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;

namespace SSO.Business.ApplicationPermissions.Handlers
{
    public class CreateAppPermissionCommandHandler : IRequestHandler<CreateAppPermissionCommand, AppPermissionDto>
    {
        readonly IApplicationPermissionRepository _applicationPermissionRepository;
        readonly IMapper _mapper;

        public CreateAppPermissionCommandHandler(IApplicationPermissionRepository applicationPermissionRepository, IMapper mapper)
        {
            _applicationPermissionRepository = applicationPermissionRepository;
            _mapper = mapper;
        }

        public async Task<AppPermissionDto> Handle(CreateAppPermissionCommand request, CancellationToken cancellationToken)
        {
            var rec = _mapper.Map<ApplicationPermission>(request);

            var res = await _applicationPermissionRepository.Add(rec);

            return _mapper.Map<AppPermissionDto>(res);
        }
    }
}
