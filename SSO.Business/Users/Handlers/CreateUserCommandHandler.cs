using AutoMapper;
using MediatR;
using SSO.Business.Users.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;

namespace SSO.Business.Users.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        readonly IUserRepository _userRepository;
        readonly IMapper _mapper;

        public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var rec = _mapper.Map<ApplicationUser>(request);
            rec.CreatedBy = request.Author!;
            rec.DateCreated = DateTime.Now;
            rec.ModifiedBy = request.Author!;
            rec.DateModified = rec.DateCreated;

            var res = await _userRepository.Add(rec);

            return _mapper.Map<UserDto>(res);
        }
    }
}
