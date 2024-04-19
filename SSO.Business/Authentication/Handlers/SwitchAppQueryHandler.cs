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
        readonly IUserRepository _userRepo;
        readonly IUserRoleRepository _userRoleRepo;
        readonly IGroupRoleRepository _groupRoleRepo;
        readonly UserManager<ApplicationUser> _userManager;

        public SwitchAppQueryHandler(ITokenService tokenService,
            IApplicationRoleRepository roleRepo, IUserRepository userRepo, IUserRoleRepository userRoleRepo, IGroupRoleRepository groupRoleRepository
            UserManager<ApplicationUser> userManager)
        {
            _tokenService = tokenService;
            _roleRepo = roleRepo;
            _userRepo = userRepo;
            _userRoleRepo = userRoleRepo;
            _userManager = userManager;
            _groupRoleRepo = groupRoleRepository;
        }

        public async Task<TokenDto> Handle(SwitchAppQuery request, CancellationToken cancellationToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadToken(request.Token) as JwtSecurityToken;

            var userId = jsonToken!.Claims.First(x => x.Type == "nameid").Value;

            var user = await _userRepo.FindOne(x => x.Id == userId);

            if (user is null)
                throw new UnauthorizedAccessException("User not found.");

            var apps = await _userRepo.GetApplications(new Guid(userId));
            if (!apps.Any(x => x.ApplicationId == request.ApplicationId))
                throw new UnauthorizedAccessException("User doesn't have access to the app.");

            if (user.DateInactive != null)
                throw new UnauthorizedAccessException("Account is inactive");

            if (await _userManager.IsLockedOutAsync(user))
                throw new UnauthorizedAccessException("User is locked-out.");

            var roles = await _userRoleRepo.Roles(user.UserName, request.ApplicationId.Value);

            var groups = await _userRepo.GetGroups(new Guid(user.Id));
            foreach (var group in groups)
                roles = roles.Union(await _groupRoleRepo.Roles(group.GroupId, request.ApplicationId.Value));

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}")
            };

            if (user.Email is not null)
                claims.Add(new Claim(ClaimTypes.Email, user.Email));

            if (!roles.Any())
                throw new UnauthorizedAccessException();

            foreach (var role in roles.ToList())
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name!));

                claims.AddRange(await _roleRepo.GetClaims(new Guid(role.Id)));
            }

            var expires = jsonToken.ValidTo;
            var token = _tokenService.GenerateToken(new ClaimsIdentity(claims), expires);

            return new TokenDto { AccessToken = token, Expires = expires };
        }
    }
}
