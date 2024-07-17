using AutoMapper;
using MediatR;
using SSO.Business.Users.Commands;
using SSO.Domain.Models;

namespace SSO.Business.Users.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        readonly RepositoryFactory _repoFactory;
        readonly IMapper _mapper;

        public UpdateUserCommandHandler(RepositoryFactory repoFactory, IMapper mapper)
        {
            _repoFactory = repoFactory;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userRepo = await _repoFactory.GetRepository(request.RealmId);

            if (!(await userRepo.Any(x => x.Id == request.UserId)))
                throw new ArgumentNullException(message: "Cannot find user", paramName: "UserNotFound");

            var rec = _mapper.Map<ApplicationUser>(request);

            var res = await userRepo.Update(rec);

            return _mapper.Map<UserDto>(res);
        }
    }
}
