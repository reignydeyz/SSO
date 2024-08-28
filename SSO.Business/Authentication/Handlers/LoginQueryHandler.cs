using MediatR;
using SSO.Business.Authentication.Queries;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Management.Interfaces;
using System.Security.Claims;

namespace SSO.Business.Authentication.Handlers
{
    /// <summary>
    /// Handler for login request
    /// </summary>
    public class LoginQueryHandler : IRequestHandler<LoginQuery, TokenDto>
    {
        readonly ITokenService _tokenService;
        readonly IApplicationRepository _appRepo;
        readonly IApplicationRoleRepository _roleRepo;
        readonly IUserRoleRepository _userRoleRepo;
        readonly IGroupRoleRepository _groupRoleRepo;
        readonly ServiceFactory _authServiceFactory;
        readonly Users.RepositoryFactory _userRepoFactory;

        public LoginQueryHandler(ITokenService tokenService, IApplicationRepository appRepo, IApplicationRoleRepository roleRepo, 
            IUserRoleRepository userRoleRepo, IGroupRoleRepository groupRoleRepo, 
            ServiceFactory authServiceFactory, Users.RepositoryFactory userRepoFactory)
        {
            _tokenService = tokenService;
            _appRepo = appRepo;
            _roleRepo = roleRepo;
            _userRoleRepo = userRoleRepo;
            _groupRoleRepo = groupRoleRepo;
            _authServiceFactory = authServiceFactory;
            _userRepoFactory = userRepoFactory;
        }

        public async Task<TokenDto> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var app = await _appRepo.FindOne(x => x.ApplicationId == request.ApplicationId);

            var authenticationService = await _authServiceFactory.GetService(app.RealmId);
            var userRepo = await _userRepoFactory.GetRepository(app.RealmId);

            await authenticationService.Login(request.Username, request.Password, app);

            var user = await userRepo.GetByUsername(request.Username);
            var roles = await _userRoleRepo.Roles(request.Username, request.ApplicationId.Value);

            var groups = await userRepo.GetGroups(new Guid(user.Id));
            foreach (var group in groups)
                roles = roles.Union(await _groupRoleRepo.Roles(group.GroupId, app.ApplicationId));

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
                new Claim("app", app.ApplicationId.ToString())
            };

            if (user.Email is not null)
                claims.Add(new Claim(ClaimTypes.Email, user.Email));

            if (!roles.Any())
                throw new UnauthorizedAccessException();

            foreach (var role in roles.ToList())
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name!));

                var permissions = await _roleRepo.GetClaims(new Guid(role.Id));

                foreach (var p in permissions)
                    claims.Add(new Claim("permissions", p.Value));
            }

            var expires = DateTime.Now.AddMinutes(app.TokenExpiration);
            var token = _tokenService.GenerateToken(new ClaimsIdentity(claims), expires, app.Name);

            return new TokenDto { AccessToken = token, Expires = expires };
        }
    }
}
