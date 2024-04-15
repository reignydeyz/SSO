using AutoMapper;
using MediatR;
using SSO.Business.Applications;
using SSO.Business.GroupApplications.Queries;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.GroupApplications.Handlers
{
    public class GetGroupAppsQueryHandler : IRequestHandler<GetGroupAppsQuery, IEnumerable<ApplicationDto>>
    {
        readonly IGroupRepository _groupRepository;
        readonly IMapper _mapper;

        public GetGroupAppsQueryHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationDto>> Handle(GetGroupAppsQuery request, CancellationToken cancellationToken)
        {
            var res = await _groupRepository.GetApplications(request.GroupId);

            return _mapper.Map<IEnumerable<ApplicationDto>>(res.OrderBy(x => x.Name));
        }
    }
}
