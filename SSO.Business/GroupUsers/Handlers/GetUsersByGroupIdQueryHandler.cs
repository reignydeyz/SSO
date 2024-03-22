using AutoMapper;
using MediatR;
using SSO.Business.GroupUsers.Queries;
using SSO.Business.Users;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.GroupUsers.Handlers
{
    public class GetUsersByGroupIdQueryHandler : IRequestHandler<GetUsersByGroupIdQuery, IQueryable<UserDto>>
    {
        readonly IGroupUserRepository _groupUserRepository;
        readonly IMapper _mapper;

        public GetUsersByGroupIdQueryHandler(IGroupUserRepository groupUserRepository, IMapper mapper)
        {
            _groupUserRepository = groupUserRepository;
            _mapper = mapper;
        }

        public async Task<IQueryable<UserDto>> Handle(GetUsersByGroupIdQuery request, CancellationToken cancellationToken)
        {
            var res = await _groupUserRepository.Find(x => x.GroupId == request.GroupId);

            return _mapper.ProjectTo<UserDto>(res.Select(x => x.User));
        }
    }
}
