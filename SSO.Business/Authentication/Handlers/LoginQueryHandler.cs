using MediatR;
using SSO.Business.Authentication.Queries;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.UserManagement.Interfaces;
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
            await _authenticationService.Login(request.Username, request.Password);

            var app = await _appRepo.FindOne(x => x.ApplicationId == request.AppId);
            var user = await _userRepo.GetByEmail(request.Username);
            var roles = await _userRoleRepo.Roles(request.Username, request.AppId.Value);

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email)
            };

            if (!roles.Any())
                throw new UnauthorizedAccessException();

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role.Name!));

            claims.AddRange((await _userClaimRepo.GetClaims(new Guid(user.Id), request.AppId.Value)).ToList());

            var expires = DateTime.Now.AddMinutes(app.TokenExpiration);
            var token = _tokenService.GenerateToken(new ClaimsIdentity(claims), expires);

            return new TokenDto { AccessToken = token, Expires = expires };
        }
    }
}
