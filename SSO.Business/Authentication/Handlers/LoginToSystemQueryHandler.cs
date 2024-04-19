using MediatR;
using SSO.Business.Authentication.Queries;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using SSO.Infrastructure.Settings.Enums;
using SSO.Infrastructure.Settings.Services;
using System.Security.Claims;

namespace SSO.Business.Authentication.Handlers
{
    public class LoginToSystemQueryHandler : IRequestHandler<LoginToSystemQuery, TokenDto>
    {
        readonly Realm _realm;
        readonly IAuthenticationService _authenticationService;
        readonly ITokenService _tokenService;
        readonly IApplicationRoleRepository _roleRepo;
        readonly IUserRepository _userRepo;
        readonly IUserRoleRepository _userRoleRepo;
        readonly IGroupRoleRepository _groupRoleRepo;
        readonly Application _root;

        public LoginToSystemQueryHandler(RealmService realmService,
            IAuthenticationService authenticationService, ITokenService tokenService,
            IApplicationRepository applicationRepository, IApplicationRoleRepository roleRepo,
            IUserRepository userRepo, IUserRoleRepository userRoleRepo, IGroupRoleRepository groupRoleRepository)
        {
            _realm = realmService.Realm;
            _authenticationService = authenticationService;
            _tokenService = tokenService;
            _roleRepo = roleRepo;
            _userRepo = userRepo;
            _userRoleRepo = userRoleRepo;
            _groupRoleRepo = groupRoleRepository;
            _root = applicationRepository.FindOne(x => x.Name == "root").Result;
        }

        public async Task<TokenDto> Handle(LoginToSystemQuery request, CancellationToken cancellationToken)
        {
            await _authenticationService.Login(request.Username, request.Password, _root);

            var user = await _userRepo.GetByUsername(request.Username);

            // Checks root access
            var roles = await _userRoleRepo.Roles(request.Username, _root.ApplicationId);

            var groups = await _userRepo.GetGroups(new Guid(user.Id));
            foreach (var group in groups)
                roles = roles.Union(await _groupRoleRepo.Roles(group.GroupId, _root.ApplicationId));

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.AuthenticationMethod, _realm.ToString()),
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}")
            };

            if (user.Email is not null)
                claims.Add(new Claim(ClaimTypes.Email, user.Email));

            // Has root access
            if (roles.Any())
            {
                claims.Add(new Claim(ClaimTypes.System, _root.ApplicationId.ToString()));

                foreach (var role in roles.ToList())
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name!));

                    claims.AddRange(await _roleRepo.GetClaims(new Guid(role.Id)));
                }
            }

            var expires = DateTime.Now.AddMinutes(_root.TokenExpiration);
            var token = _tokenService.GenerateToken(new ClaimsIdentity(claims), expires);

            return new TokenDto { AccessToken = token, Expires = expires };
        }
    }
}
