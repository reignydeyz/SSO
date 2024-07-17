using AutoMapper;
using MediatR;
using SSO.Business.Groups.Queries;

namespace SSO.Business.Groups.Handlers
{
    public class GetGroupsQueryHandler : IRequestHandler<GetGroupsQuery, IQueryable<GroupDto>>
    {
        readonly RepositoryFactory _groupRepoFactory;
        readonly IMapper _mapper;

        public GetGroupsQueryHandler(RepositoryFactory groupRepoFactory, IMapper mapper)
        {
            _groupRepoFactory = groupRepoFactory;
            _mapper = mapper;
        }

        public async Task<IQueryable<GroupDto>> Handle(GetGroupsQuery request, CancellationToken cancellationToken)
        {
            var groupRepo = await _groupRepoFactory.GetRepository();

            var res = await groupRepo.Find(default);

            return _mapper.ProjectTo<GroupDto>(res);
        }
    }
}
