using MediatR;
using SSO.Business.Authentication.Queries;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Security.Claims;

namespace SSO.Business.Authentication.Handlers
{
    public class LoginToSystemQueryHandler : IRequestHandler<LoginToSystemQuery, TokenDto>
    {
        readonly IAuthenticationService _authenticationService;
        readonly ITokenService _tokenService;
        readonly IUserRepository _userRepo;
        readonly IUserRoleRepository _userRoleRepo;
        readonly IUserClaimRepository _userClaimRepository;
        readonly Application _root;

        public LoginToSystemQueryHandler(IAuthenticationService authenticationService, ITokenService tokenService,
            IApplicationRepository applicationRepository,
            IUserRepository userRepo, IUserRoleRepository userRoleRepo, IUserClaimRepository userClaimRepository)
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
            _userRepo = userRepo;
            _userRoleRepo = userRoleRepo;
            _userClaimRepository = userClaimRepository;
            _root = applicationRepository.FindOne(x => x.Name == "root").Result;
        }

        public async Task<TokenDto> Handle(LoginToSystemQuery request, CancellationToken cancellationToken)
        {
            await _authenticationService.Login(request.Username, request.Password, _root);

            var user = await _userRepo.GetByUsername(request.Username);

            // Checks root access
            var roles = await _userRoleRepo.Roles(request.Username, _root.ApplicationId);

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}")
            };

            if (user.Email is not null)
                claims.Add(new Claim(ClaimTypes.Email, user.Email));

            // Has root access
            if (roles.Any())
            {
                claims.Add(new Claim(ClaimTypes.System, _root.ApplicationId.ToString()));

                foreach (var role in roles)
                    claims.Add(new Claim(ClaimTypes.Role, role.Name!));

                claims.AddRange((await _userClaimRepository.GetClaims(new Guid(user.Id), _root.ApplicationId)).ToList());
            }

            var expires = DateTime.Now.AddMinutes(_root.TokenExpiration);
            var token = _tokenService.GenerateToken(new ClaimsIdentity(claims), expires);

            return new TokenDto { AccessToken = token, Expires = expires };
        }
    }
}
