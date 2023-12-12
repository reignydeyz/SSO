using MediatR;
using SSO.Business.Authentication.Queries;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using SSO.Domain.UserManagement.Interfaces;
using System.Security.Claims;

namespace SSO.Business.Authentication.Handlers
{
    public class LoginAsRootQueryHandler : IRequestHandler<LoginAsRootQuery, string>
    {
        readonly IAuthenticationService _authenticationService;
        readonly ITokenService _tokenService;
        readonly IUserRepository _userRepo;
        readonly IUserRoleRepository _userRoleRepo;
        readonly IUserClaimRepository _userClaimRepository;
        readonly Application _root;

        public LoginAsRootQueryHandler(IAuthenticationService authenticationService, ITokenService tokenService,
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

        public async Task<string> Handle(LoginAsRootQuery request, CancellationToken cancellationToken)
        {
            await _authenticationService.Login(request.Username, request.Password);

            var user = await _userRepo.GetByEmail(request.Username);
            var roles = await _userRoleRepo.Roles(request.Username, _root.ApplicationId);

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email)
            };

            if (!roles.Any())
                throw new UnauthorizedAccessException();

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role.Name!));

            claims.AddRange((await _userClaimRepository.GetClaims(new Guid(user.Id), _root.ApplicationId)).ToList());

            var token = _tokenService.GenerateToken(new ClaimsIdentity(claims));

            return token;
        }
    }
}
