using MediatR;
using Microsoft.AspNetCore.Identity;
using SSO.Business.Authentication.Queries;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SSO.Business.Authentication.Handlers
{
    public class SwitchAppQueryHandler : IRequestHandler<SwitchAppQuery, TokenDto>
    {
        readonly ITokenService _tokenService;
        readonly IApplicationRoleRepository _roleRepo;
        readonly IUserRoleRepository _userRoleRepo;
        readonly IGroupRoleRepository _groupRoleRepo;
        readonly UserManager<ApplicationUser> _userManager;
        readonly Users.RepositoryFactory _userRepoFactory;

        public SwitchAppQueryHandler(ITokenService tokenService,
            IApplicationRoleRepository roleRepo, IUserRoleRepository userRoleRepo, IGroupRoleRepository groupRoleRepository,
            UserManager<ApplicationUser> userManager, Users.RepositoryFactory userRepoFactory)
        {
            _tokenService = tokenService;
            _roleRepo = roleRepo;
            _userRoleRepo = userRoleRepo;
            _userManager = userManager;
            _groupRoleRepo = groupRoleRepository;
            _userRepoFactory = userRepoFactory;
        }

        public async Task<TokenDto> Handle(SwitchAppQuery request, CancellationToken cancellationToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadToken(request.Token) as JwtSecurityToken;

            var userId = jsonToken!.Claims.First(x => x.Type == "nameid").Value;

            var userRepo = await _userRepoFactory.GetRepository();

            var user = await userRepo.FindOne(x => x.Id == userId);

            if (user is null)
                throw new UnauthorizedAccessException("User not found.");

            var apps = await userRepo.GetApplications(new Guid(userId));
            var app = apps.FirstOrDefault(x => x.ApplicationId == request.ApplicationId);

            if (app is null)
                throw new UnauthorizedAccessException("User doesn't have access to the app.");

            if (user.DateInactive != null)
                throw new UnauthorizedAccessException("Account is inactive");

            if (await _userManager.IsLockedOutAsync(user))
                throw new UnauthorizedAccessException("User is locked-out.");

            var roles = await _userRoleRepo.Roles(user.UserName, request.ApplicationId.Value);

            var groups = await userRepo.GetGroups(new Guid(user.Id));
            foreach (var group in groups)
                roles = roles.Union(await _groupRoleRepo.Roles(group.GroupId, request.ApplicationId.Value));

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
                new Claim("realm", app.RealmId.ToString()),
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

            var expires = jsonToken.ValidTo;
            var token = _tokenService.GenerateToken(new ClaimsIdentity(claims), expires);

            return new TokenDto { AccessToken = token, Expires = expires };
        }
    }
}
