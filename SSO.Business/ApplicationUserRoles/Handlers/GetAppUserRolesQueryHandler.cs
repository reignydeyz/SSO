using AutoMapper;
using MediatR;
using SSO.Business.ApplicationRoles;
using SSO.Business.ApplicationUserRoles.Queries;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.ApplicationUserRoles.Handlers
{
    public class GetAppUserRolesQueryHandler : IRequestHandler<GetAppUserRolesQuery, IEnumerable<AppRoleDto>>
    {
        readonly IUserRoleRepository _userRoleRepository;
        readonly IMapper _mapper;

        public GetAppUserRolesQueryHandler(IUserRoleRepository userRoleRepository, IMapper mapper)
        {
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppRoleDto>> Handle(GetAppUserRolesQuery request, CancellationToken cancellationToken)
        {
            var res = await _userRoleRepository.Roles(request.UserId, request.ApplicationId);

            return _mapper.Map<IEnumerable<AppRoleDto>>(res.OrderBy(x => x.Name));
        }
    }
}
