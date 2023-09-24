using MediatR;
using SSO.Business.Authentication.Queries;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.UserManegement.Interfaces;

namespace SSO.Business.Authentication.Handlers
{
    /// <summary>
    /// Handler for login request
    /// </summary>
    public class LoginQueryHandler : IRequestHandler<LoginQuery, string>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserRepository _userManagementService;
        private readonly IUserRoleRepository _userRoleManagementService;

        public LoginQueryHandler(IAuthenticationService authenticationService, IUserRepository userManagementService, IUserRoleRepository userRoleManagementService)
        {
            _authenticationService = authenticationService;
            _userManagementService = userManagementService;
            _userRoleManagementService = userRoleManagementService;
        }

        public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            await _authenticationService.Login(request.Username, request.Password);

            var user = await _userManagementService.GetByEmail(request.Username);
            var roles = await _userRoleManagementService.Roles(request.Username);
            
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

            // TODO: Generate access token
            var token = string.Empty;

            return token;
        }
    }
}
