using AutoMapper;
using MediatR;
using SSO.Business.Applications;
using SSO.Business.GroupApplications.Queries;
using SSO.Business.Groups;

namespace SSO.Business.GroupApplications.Handlers
{
    public class GetGroupAppsQueryHandler : IRequestHandler<GetGroupAppsQuery, IEnumerable<ApplicationDto>>
    {
        readonly RepositoryFactory _groupRepositoryFactory;
        readonly IMapper _mapper;

        public GetGroupAppsQueryHandler(RepositoryFactory groupRepositoryFactory, IMapper mapper)
        {
            _groupRepositoryFactory = groupRepositoryFactory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationDto>> Handle(GetGroupAppsQuery request, CancellationToken cancellationToken)
        {
            var repo = await _groupRepositoryFactory.GetRepository();

            var res = await repo.GetApplications(request.GroupId);

            return _mapper.Map<IEnumerable<ApplicationDto>>(res.OrderBy(x => x.Name));
        }
    }
}
