using AutoMapper;
using MediatR;
using SSO.Business.Groups.Queries;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.Groups.Handlers
{
    public class GetGroupsQueryHandler : IRequestHandler<GetGroupsQuery, IQueryable<GroupDto>>
    {
        readonly IGroupRepository _groupRepository;
        readonly IMapper _mapper;

        public GetGroupsQueryHandler(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        public async Task<IQueryable<GroupDto>> Handle(GetGroupsQuery request, CancellationToken cancellationToken)
        {
            var res = await _groupRepository.Find(default);

            return _mapper.ProjectTo<GroupDto>(res);
        }
    }
}
