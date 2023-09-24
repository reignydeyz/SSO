using MediatR;
using SSO.Business.Authentication.Queries;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.UserManegement.Interfaces;
using System.Security.Claims;

namespace SSO.Business.Authentication.Handlers
{
    /// <summary>
    /// Handler for login request
    /// </summary>
    public class LoginQueryHandler : IRequestHandler<LoginQuery, string>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserRepository _userRepo;
        private readonly IUserRoleRepository _userRoleRepo;
        private readonly IRoleRepository _roleRepository;

        public LoginQueryHandler(IAuthenticationService authenticationService, IUserRepository userRepo, IUserRoleRepository userRoleRepo, IRoleRepository roleRepository)
        {
            _authenticationService = authenticationService;
            _userRepo = userRepo;
            _userRoleRepo = userRoleRepo;
            _roleRepository = roleRepository;
        }

        public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            await _authenticationService.Login(request.Username, request.Password);

            var user = await _userRepo.GetByEmail(request.Username);
            var roles = await _userRoleRepo.Roles(request.Username);
            
            if (request.ApplicationId.HasValue)
            {
                if (!roles.Any(x => x.ApplicationId == request.ApplicationId))
                    throw new UnauthorizedAccessException();

                // Apply roles for the specific application
                roles = roles.Where(x => x.ApplicationId == request.ApplicationId);
            }
            else
            {
                // Check root access
                if (!roles.Any(x => x.Application.Name == "root"))
                    throw new UnauthorizedAccessException();

                // Apply roles for root
                roles = roles.Where(x => x.Application.Name == "root");
            }

            var claims = new List<Claim>();

            foreach (var role in roles)
            {
                // TODO: Use AspNetUserClaims
                // Get the claims for the role
                var roleClaims = await _roleRepository.GetClaims(new Guid(role.Id));

                // Add the retrieved claims to the list
                claims.AddRange(roleClaims);
            }

            // TODO: Generate access token
            var token = string.Empty;

            return token;
        }
    }
}
