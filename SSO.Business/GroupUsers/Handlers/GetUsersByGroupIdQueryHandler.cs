using AutoMapper;
using MediatR;
using SSO.Business.GroupUsers.Queries;
using SSO.Business.Users;

namespace SSO.Business.GroupUsers.Handlers
{
    public class GetUsersByGroupIdQueryHandler : IRequestHandler<GetUsersByGroupIdQuery, IQueryable<UserDto>>
    {
        readonly RepositoryFactory _groupUserRepoFactory;
        readonly IMapper _mapper;

        public GetUsersByGroupIdQueryHandler(RepositoryFactory groupUserRepoFactory, IMapper mapper)
        {
            _groupUserRepoFactory = groupUserRepoFactory;
            _mapper = mapper;
        }

        public async Task<IQueryable<UserDto>> Handle(GetUsersByGroupIdQuery request, CancellationToken cancellationToken)
        {
            var repo = await _groupUserRepoFactory.GetRepository(request.RealmId);

            var res = await repo.Find(x => x.GroupId == request.GroupId);

            return _mapper.ProjectTo<UserDto>(res.Select(x => x.User));
        }
    }
}
