using AutoMapper;
using MediatR;
using SSO.Business.Applications;
using SSO.Business.UserApplications.Queries;
using SSO.Business.Users;

namespace SSO.Business.UserApplications.Handlers
{
    public class GetUserAppsQueryHandler : IRequestHandler<GetUserAppsQuery, IEnumerable<ApplicationDto>>
    {
        readonly RepositoryFactory _userRepoFactory;
        readonly IMapper _mapper;

        public GetUserAppsQueryHandler(RepositoryFactory userRepoFactory, IMapper mapper)
        {
            _userRepoFactory = userRepoFactory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationDto>> Handle(GetUserAppsQuery request, CancellationToken cancellationToken)
        {
            var userRepo = await _userRepoFactory.GetRepository();

            var res = await userRepo.GetApplications(request.UserId);

            return _mapper.Map<IEnumerable<ApplicationDto>>(res.OrderBy(x => x.Name));
        }
    }
}
