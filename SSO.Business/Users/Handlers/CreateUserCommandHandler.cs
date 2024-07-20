using AutoMapper;
using MediatR;
using SSO.Business.Users.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;

namespace SSO.Business.Users.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        readonly RepositoryFactory _repoFactory;
        readonly IRealmUserRepository _realmUserRepository;
        readonly IMapper _mapper;

        public CreateUserCommandHandler(RepositoryFactory repoFactory, IRealmUserRepository realmUserRepository, IMapper mapper)
        {
            _repoFactory = repoFactory;
            _realmUserRepository = realmUserRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userRepo = await _repoFactory.GetRepository(request.RealmId);

            var user = await userRepo.FindOne(x => x.UserName == request.Username);

            if (user is null)
            {
                var rec = _mapper.Map<ApplicationUser>(request);
                rec.CreatedBy = request.Author!;
                rec.DateCreated = DateTime.Now;
                rec.ModifiedBy = request.Author!;
                rec.DateModified = rec.DateCreated;

                user = await userRepo.Add(rec);
            }

            await _realmUserRepository.Add(new RealmUser { RealmId = request.RealmId, UserId = user.Id });

            return _mapper.Map<UserDto>(user);
        }
    }
}
