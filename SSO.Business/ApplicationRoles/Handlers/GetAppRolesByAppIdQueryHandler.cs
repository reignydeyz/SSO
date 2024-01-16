using AutoMapper;
using MediatR;
using SSO.Business.ApplicationRoles.Queries;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.ApplicationRoles.Handlers
{
    public class GetAppRolesByAppIdQueryHandler : IRequestHandler<GetAppRolesByAppIdQuery, IEnumerable<AppRoleDto>>
    {
        readonly IApplicationRepository _applicationRepository;
        readonly IMapper _mapper;

        public GetAppRolesByAppIdQueryHandler(IApplicationRepository applicationRepository, IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppRoleDto>> Handle(GetAppRolesByAppIdQuery request, CancellationToken cancellationToken)
        {
            var res = await _applicationRepository.GetRoles(request.ApplicationId);

            return _mapper.Map<IEnumerable<AppRoleDto>>(res);
        }
    }
}
