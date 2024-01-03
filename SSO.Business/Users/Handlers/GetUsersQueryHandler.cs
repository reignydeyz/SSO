using AutoMapper;
using MediatR;
using SSO.Business.Users.Queries;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.Users.Handlers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IQueryable<UserDto>>
    {
        readonly IMapper _mapper;
        readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<IQueryable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var res = await _userRepository.Find(default);

            return _mapper.ProjectTo<UserDto>(res);
        }
    }
}
