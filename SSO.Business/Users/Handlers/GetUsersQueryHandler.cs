using AutoMapper;
using MediatR;
using SSO.Business.Users.Queries;

namespace SSO.Business.Users.Handlers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IQueryable<UserDto>>
    {
        readonly IMapper _mapper;
        readonly RepositoryFactory _repoFactory;

        public GetUsersQueryHandler(IMapper mapper, RepositoryFactory repoFactory)
        {
            _mapper = mapper;
            _repoFactory = repoFactory;
        }

        public async Task<IQueryable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var userRepo = await _repoFactory.GetRepository(request.RealmId);

            var res = await userRepo.Find(x => x.Realms.Any(x => x.RealmId == request.RealmId));

            return _mapper.ProjectTo<UserDto>(res);
        }
    }
}
