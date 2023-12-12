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
    public class LoginQueryHandler : IRequestHandler<LoginQuery, string>
    {
        readonly IAuthenticationService _authenticationService;
        readonly ITokenService _tokenService;
        readonly IUserRepository _userRepo;
        readonly IUserRoleRepository _userRoleRepo;
        readonly IUserClaimRepository _userClaimRepository;

        public LoginQueryHandler(IAuthenticationService authenticationService, ITokenService tokenService,
            IUserRepository userRepo, IUserRoleRepository userRoleRepo, IUserClaimRepository userClaimRepository)
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
            _userRepo = userRepo;
            _userRoleRepo = userRoleRepo;
            _userClaimRepository = userClaimRepository;
        }

        public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            await _authenticationService.Login(request.Username, request.Password);

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

            claims.AddRange((await _userClaimRepository.GetClaims(new Guid(user.Id), request.AppId.Value)).ToList());

            var token = _tokenService.GenerateToken(new ClaimsIdentity(claims));

            return token;
        }
    }
}
