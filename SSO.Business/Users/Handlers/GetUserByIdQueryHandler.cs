using AutoMapper;
using MediatR;
using SSO.Business.Users.Queries;

namespace SSO.Business.Users.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDetailDto>
    {
        readonly RepositoryFactory _repoFactory;
        readonly IMapper _mapper;

        public GetUserByIdQueryHandler(RepositoryFactory repoFactory, IMapper mapper)
        {
            _repoFactory = repoFactory;
            _mapper = mapper;
        }

        public async Task<UserDetailDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var userRepo = await _repoFactory.GetRepository(request.RealmId);

            var rec = await userRepo.FindOne(x => x.Id == request.UserId.ToString());

            if (rec is null)
                throw new ArgumentNullException();

            return _mapper.Map<UserDetailDto>(rec);
        }
    }
}
