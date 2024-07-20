using AutoMapper;
using MediatR;
using SSO.Business.Users.Commands;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;

namespace SSO.Business.Users.Handlers
{
    public class CopyUserCommandHandler : IRequestHandler<CopyUserCommand, UserDto>
    { 
        readonly IUserRoleRepository _userRoleRepository;
        readonly IRealmUserRepository _realmUserRepository;
        readonly IMapper _mapper;
        readonly RepositoryFactory _userRepoFactory;
        readonly GroupUsers.RepositoryFactory _groupRepoFactory;

        public CopyUserCommandHandler(IUserRoleRepository userRoleRepository, 
            IRealmUserRepository realmUserRepository,
            IMapper mapper,
            RepositoryFactory userRepoFactory,
            GroupUsers.RepositoryFactory groupRepoFactory)
        {
            _userRoleRepository = userRoleRepository;
            _realmUserRepository = realmUserRepository;
            _mapper = mapper;
            _userRepoFactory = userRepoFactory;
            _groupRepoFactory = groupRepoFactory;
        }

        public async Task<UserDto> Handle(CopyUserCommand request, CancellationToken cancellationToken)
        {
            var userRepo = await _userRepoFactory.GetRepository(request.RealmId);
            var groupUserRepo = await _groupRepoFactory.GetRepository(request.RealmId);

            var srcUser = await userRepo.FindOne(x => x.Id == request.UserId);

            ApplicationUser? existingUser = null;

            existingUser = await userRepo.FindOne(x => x.UserName == request.Username);

            var newUser = _mapper.Map<ApplicationUser>(request);
            newUser.Id = existingUser?.Id ?? Guid.NewGuid().ToString();
            newUser.CreatedBy = existingUser?.CreatedBy ?? request.Author!;
            newUser.DateCreated = existingUser?.DateCreated ?? DateTime.Now;
            newUser.ModifiedBy = request.Author!;
            newUser.DateModified = DateTime.Now;

            if (existingUser == null)
            {
                await userRepo.Add(newUser);
                await _realmUserRepository.Add(new RealmUser { RealmId = request.RealmId, UserId = newUser.Id });
            }
            else
                await userRepo.Update(newUser);

            // Apps
            var apps = await userRepo.GetApplications(new Guid(srcUser.Id));

            // Clear apps associated to the user being updated (existing user)
            if (existingUser != null)
            {
                var exApps = await userRepo.GetApplications(new Guid(existingUser.Id));

                foreach (var app in exApps)
                {
                    var roles = await _userRoleRepository.Roles(new Guid(existingUser.Id), app.ApplicationId);
                    await _userRoleRepository.RemoveRoles(new Guid(existingUser.Id), roles);
                }
            }

            // User roles
            var newRoles = new List<ApplicationRole>();
            foreach (var app in apps)
            {
                var roles = await _userRoleRepository.Roles(new Guid(srcUser.Id), app.ApplicationId);

                if (existingUser != null)
                    await _userRoleRepository.RemoveRoles(new Guid(existingUser.Id), roles);

                newRoles.AddRange(roles);
            }
            await _userRoleRepository.AddRoles(new Guid(newUser.Id), newRoles);

            // User groups
            var groups = await userRepo.GetGroups(new Guid(srcUser.Id));
            if (existingUser != null)
                await groupUserRepo.RemoveRange(x => x.UserId == existingUser.Id, false);

            await groupUserRepo.AddRange(groups.Select(x => new GroupUser { GroupId = x.GroupId, UserId = newUser.Id }));

            return _mapper.Map<UserDto>(newUser);
        }
    }
}
