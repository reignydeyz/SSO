using AutoMapper;
using MediatR;
using SSO.Business.Users.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;

namespace SSO.Business.Users.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        readonly IUserRepository _userRepository;
        readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (!(await _userRepository.Any(x => x.Id == request.UserId)))
                throw new ArgumentNullException(message: "Cannot find user", paramName: "UserNotFound");

            var rec = _mapper.Map<ApplicationUser>(request);

            var res = await _userRepository.Update(rec);

            return _mapper.Map<UserDto>(res);
        }
    }
}
