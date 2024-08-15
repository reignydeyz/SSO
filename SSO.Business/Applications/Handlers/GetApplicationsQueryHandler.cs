using AutoMapper;
using MediatR;
using SSO.Business.Applications.Queries;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.Applications.Handlers
{
    public class GetApplicationsQueryHandler : IRequestHandler<GetApplicationsQuery, IQueryable<ApplicationDto>>
    {
        readonly IApplicationRepository _applicationRepository;
        readonly IMapper _mapper;

        public GetApplicationsQueryHandler(IApplicationRepository applicationRepository, IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
        }

        public async Task<IQueryable<ApplicationDto>> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
        {
            var res = await _applicationRepository.Find(x => x.RealmId == request.RealmId);

            return _mapper.ProjectTo<ApplicationDto>(res);
        }
    }
}
