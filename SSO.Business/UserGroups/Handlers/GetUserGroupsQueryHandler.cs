using AutoMapper;
using MediatR;
using SSO.Business.Groups;
using SSO.Business.UserGroups.Queries;

namespace SSO.Business.UserGroups.Handlers
{
    public class GetUserGroupsQueryHandler : IRequestHandler<GetUserGroupsQuery, IEnumerable<GroupDto>>
    {
        readonly Users.RepositoryFactory _userRepoFactory;
        readonly IMapper _mapper;

        public GetUserGroupsQueryHandler(Users.RepositoryFactory userRepoFactory, IMapper mapper)
        {
            _userRepoFactory = userRepoFactory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GroupDto>> Handle(GetUserGroupsQuery request, CancellationToken cancellationToken)
        {
            var userRepo = await _userRepoFactory.GetRepository();

            var res = await userRepo.GetGroups(request.UserId);

            return _mapper.Map<IEnumerable<GroupDto>>(res.OrderBy(x => x.Name));
        }
    }
}
