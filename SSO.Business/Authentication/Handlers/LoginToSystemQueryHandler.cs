using AutoMapper;
using MediatR;
using SSO.Business.Authentication.Queries;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Management.Interfaces;
using System.Security.Claims;

namespace SSO.Business.Authentication.Handlers
{
    public class LoginToSystemQueryHandler : IRequestHandler<LoginToSystemQuery, TokenDto>
    {
        readonly ITokenService _tokenService;
        readonly IMapper _mapper;
        readonly IApplicationRepository _applicationRepository;
        readonly IApplicationRoleRepository _roleRepo;
        readonly IUserRoleRepository _userRoleRepo;
        readonly IGroupRoleRepository _groupRoleRepo;
        readonly IRealmUserRepository _realmUserRepo;
        readonly ServiceFactory _authServiceFactory;
        readonly Users.RepositoryFactory _userRepoFactory;

        public LoginToSystemQueryHandler(ITokenService tokenService, IMapper mapper,
            IApplicationRepository applicationRepository, IApplicationRoleRepository roleRepo, IUserRoleRepository userRoleRepo,
            IGroupRoleRepository groupRoleRepository,
            IRealmUserRepository realmUserRepository,
            ServiceFactory authServiceFactory, Users.RepositoryFactory userRepoFactory)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _applicationRepository = applicationRepository;
            _roleRepo = roleRepo;
            _userRoleRepo = userRoleRepo;
            _groupRoleRepo = groupRoleRepository;
            _realmUserRepo = realmUserRepository;
            _authServiceFactory = authServiceFactory;
            _userRepoFactory = userRepoFactory;
        }

        public async Task<TokenDto> Handle(LoginToSystemQuery request, CancellationToken cancellationToken)
        {
           var root = await _applicationRepository.FindOne(
                x => request.RealmId.HasValue ?
                        x.RealmId == request.RealmId && x.Name == "root" :
                        x.Realm.Name == "Default" && x.Name == "root"
            );

            if (root is null)
                throw new ArgumentException("Invalid realm.");

            var authenticationService = await _authServiceFactory.GetService(root.RealmId);
            var userRepo = await _userRepoFactory.GetRepository(root.RealmId);

            await authenticationService.Login(request.Username, request.Password, root);

            var user = await userRepo.GetByUsername(request.Username);

            // Checks root access
            var roles = await _userRoleRepo.Roles(request.Username, root.ApplicationId);

            var groups = await userRepo.GetGroups(new Guid(user.Id));
            foreach (var group in groups)
                roles = roles.Union(await _groupRoleRepo.Roles(group.GroupId, root.ApplicationId));

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}")
            };

            if (user.Email is not null)
                claims.Add(new Claim(ClaimTypes.Email, user.Email));

            // Has root access
            if (roles.Any())
            {
                claims.Add(new Claim(ClaimTypes.System, root.RealmId.ToString()));

                foreach (var role in roles.ToList())
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name!));

                    claims.AddRange(await _roleRepo.GetClaims(new Guid(role.Id)));
                }
            }

            var expires = DateTime.Now.AddMinutes(root.TokenExpiration);
            var token = _tokenService.GenerateToken(new ClaimsIdentity(claims), expires);

            return new TokenDto { AccessToken = token, Expires = expires };
        }
    }
}
