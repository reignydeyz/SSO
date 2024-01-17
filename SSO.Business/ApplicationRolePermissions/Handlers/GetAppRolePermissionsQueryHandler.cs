using AutoMapper;
using MediatR;
using SSO.Business.ApplicationPermissions;
using SSO.Business.ApplicationRolePermissions.Queries;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.ApplicationRolePermissions.Handlers
{
    public class GetAppRolePermissionsQueryHandler : IRequestHandler<GetAppRolePermissionsQuery, IEnumerable<AppPermissionDto>>
    {
        readonly IApplicationRoleClaimRepository _applicationRoleClaimRepository;
        readonly IMapper _mapper;

        public GetAppRolePermissionsQueryHandler(IApplicationRoleClaimRepository applicationRoleClaimRepository, IMapper mapper)
        {
            _applicationRoleClaimRepository = applicationRoleClaimRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppPermissionDto>> Handle(GetAppRolePermissionsQuery request, CancellationToken cancellationToken)
        {
            var res = await _applicationRoleClaimRepository.Find(x => x.RoleId == request.RoleId.ToString());

            return _mapper.Map<IEnumerable<AppPermissionDto>>(res);
        }
    }
}
