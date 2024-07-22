using AutoMapper;
using MediatR;
using SSO.Business.Users.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;

namespace SSO.Business.Users.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        readonly RepositoryFactory _repoFactory;
        readonly IRealmRepository _realmRepository;
        readonly IMapper _mapper;

        public UpdateUserCommandHandler(RepositoryFactory repoFactory,
            IRealmRepository realmRepository, IMapper mapper)
        {
            _repoFactory = repoFactory;
            _realmRepository = realmRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var realm = await _realmRepository.FindOne(x => x.RealmId == request.RealmId);
            var userRepo = await _repoFactory.GetRepository(request.RealmId);

            if (!(await userRepo.Any(x => x.Id == request.UserId)))
                throw new ArgumentNullException(message: "Cannot find user", paramName: "UserNotFound");

            var rec = _mapper.Map<ApplicationUser>(request);

            var res = await userRepo.Update(rec, default, realm);

            return _mapper.Map<UserDto>(res);
        }
    }
}
