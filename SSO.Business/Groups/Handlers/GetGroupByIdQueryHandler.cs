using AutoMapper;
using MediatR;
using SSO.Business.Groups.Queries;

namespace SSO.Business.Groups.Handlers
{
    public class GetGroupByIdQueryHandler : IRequestHandler<GetGroupByIdQuery, GroupDto>
    {
        readonly RepositoryFactory _groupRepoFactory;
        readonly IMapper _mapper;

        public GetGroupByIdQueryHandler(RepositoryFactory groupRepoFactory, IMapper mapper)
        {
            _groupRepoFactory = groupRepoFactory;
            _mapper = mapper;
        }

        public async Task<GroupDto> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
        {
            var groupRepo = await _groupRepoFactory.GetRepository();

            var res = await groupRepo.FindOne(x => x.GroupId == request.GroupId);

            if (res is null)
                throw new ArgumentNullException();

            return _mapper.Map<GroupDto>(res);
        }
    }
}
