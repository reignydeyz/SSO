using AutoMapper;
using MediatR;
using SSO.Business.ApplicationPermissions.Queries;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.ApplicationPermissions.Handlers
{
    public class GetAppPermissionsByAppIdQueryHandler : IRequestHandler<GetAppPermissionsByAppIdQuery, IEnumerable<AppPermissionDto>>
    {
        readonly IApplicationRepository _appRepository;
        readonly IMapper _mapper;

        public GetAppPermissionsByAppIdQueryHandler(IApplicationRepository applicationRepository, IMapper mapper)
        {
            _appRepository = applicationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppPermissionDto>> Handle(GetAppPermissionsByAppIdQuery request, CancellationToken cancellationToken)
        {
            var res = await _appRepository.GetPermissions(request.ApplicationId);

            return _mapper.Map<IEnumerable<AppPermissionDto>>(res);
        }
    }
}
