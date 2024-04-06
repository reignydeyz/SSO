using AutoMapper;
using MediatR;
using SSO.Business.ApplicationGroupRoles.Queries;
using SSO.Business.ApplicationRoles;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.ApplicationGroupRoles.Handlers
{
    public class GetAppGroupRolesQueryHandler : IRequestHandler<GetAppGroupRolesQuery, IEnumerable<AppRoleDto>>
    {
        readonly IGroupRoleRepository _groupRoleRepository;
        readonly IMapper _mapper;

        public GetAppGroupRolesQueryHandler(IGroupRoleRepository groupRoleRepository, IMapper mapper)
        {
            _groupRoleRepository = groupRoleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppRoleDto>> Handle(GetAppGroupRolesQuery request, CancellationToken cancellationToken)
        {
            var res = await _groupRoleRepository.Roles(request.GroupId, request.ApplicationId);

            return _mapper.Map<IEnumerable<AppRoleDto>>(res.OrderBy(x => x.Name));
        }
    }
}
