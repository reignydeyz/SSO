using AutoMapper;
using MediatR;
using SSO.Business.Users.Queries;
using SSO.Domain.Management.Interfaces;

namespace SSO.Business.Users.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDetailDto>
    {
        readonly IUserRepository _userRepository;
        readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDetailDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var rec = await _userRepository.FindOne(x => x.Id == request.UserId.ToString());

            if (rec is null)
                throw new ArgumentNullException();

            return _mapper.Map<UserDetailDto>(rec);
        }
    }
}
