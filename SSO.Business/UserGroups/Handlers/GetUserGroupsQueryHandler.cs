using AutoMapper;
using MediatR;
using SSO.Business.Groups;
using SSO.Business.UserGroups.Queries;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.UserGroups.Handlers
{
    public class GetUserGroupsQueryHandler : IRequestHandler<GetUserGroupsQuery, IEnumerable<GroupDto>>
    {
        readonly IUserRepository _userRepository;
        readonly IMapper _mapper;

        public GetUserGroupsQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GroupDto>> Handle(GetUserGroupsQuery request, CancellationToken cancellationToken)
        {
            var res = await _userRepository.GetGroups(request.UserId);

            return _mapper.Map<IEnumerable<GroupDto>>(res.OrderBy(x => x.Name));
        }
    }
}
