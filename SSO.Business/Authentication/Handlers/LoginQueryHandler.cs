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
        readonly IAuthenticationService _authenticationService;
        readonly ITokenService _tokenService;
        readonly IApplicationRepository _appRepo;
        readonly IUserRepository _userRepo;
        readonly IUserRoleRepository _userRoleRepo;
        readonly IUserClaimRepository _userClaimRepo;

        public LoginQueryHandler(IAuthenticationService authenticationService, ITokenService tokenService,
            IApplicationRepository appRepo, IUserRepository userRepo, IUserRoleRepository userRoleRepo, IUserClaimRepository userClaimRepo)
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
            _appRepo = appRepo;
            _userRepo = userRepo;
            _userRoleRepo = userRoleRepo;
            _userClaimRepo = userClaimRepo;
        }

        public async Task<TokenDto> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var app = await _appRepo.FindOne(x => x.ApplicationId == request.ApplicationId);

            await _authenticationService.Login(request.Username, request.Password, app);

            var user = await _userRepo.GetByUsername(request.Username);
            var roles = await _userRoleRepo.Roles(request.Username, request.ApplicationId.Value);

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}")
            };

            if (user.Email is not null)
                claims.Add(new Claim(ClaimTypes.Email, user.Email));

            if (!roles.Any())
                throw new UnauthorizedAccessException();

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role.Name!));

            claims.AddRange((await _userClaimRepo.GetClaims(new Guid(user.Id), request.ApplicationId.Value)).ToList());

            var expires = DateTime.Now.AddMinutes(app.TokenExpiration);
            var token = _tokenService.GenerateToken(new ClaimsIdentity(claims), expires, app.Name);

            return new TokenDto { AccessToken = token, Expires = expires };
        }
    }
}
