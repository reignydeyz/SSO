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
        readonly IRealmRepository _realmRepository;
        readonly IRealmUserRepository _realmUserRepository;
        readonly IMapper _mapper;

        public CreateUserCommandHandler(RepositoryFactory repoFactory, 
            IRealmRepository realmRepository, IRealmUserRepository realmUserRepository, IMapper mapper)
        {
            _repoFactory = repoFactory;
            _realmRepository = realmRepository;
            _realmUserRepository = realmUserRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var realm = await _realmRepository.FindOne(x => x.RealmId == request.RealmId);
            var userRepo = await _repoFactory.GetRepository(request.RealmId);

            var rec = _mapper.Map<ApplicationUser>(request);
                rec.CreatedBy = request.Author!;
                rec.DateCreated = DateTime.Now;
                rec.ModifiedBy = request.Author!;
                rec.DateModified = rec.DateCreated;

            var res = await userRepo.Add(rec, false, realm);

            await _realmUserRepository.Add(new RealmUser { RealmId = request.RealmId, UserId = res.Id });

            return _mapper.Map<UserDto>(res);
        }
    }
}
