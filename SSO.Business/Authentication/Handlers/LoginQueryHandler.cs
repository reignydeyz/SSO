using MediatR;
using SSO.Business.Authentication.Queries;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.UserManegement.Interfaces;

namespace SSO.Business.Authentication.Handlers {
    /// <summary>
    /// Handler for login request
    /// </summary>
    public class LoginQueryHandler : IRequestHandler<LoginQuery, string>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserRepository _userRepo;
        private readonly IUserRoleRepository _userRoleRepo;

        public LoginQueryHandler(IAuthenticationService authenticationService, IUserRepository userRepo, IUserRoleRepository userRoleRepo)
        {
            _authenticationService = authenticationService;
            _userRepo = userRepo;
            _userRoleRepo = userRoleRepo;
        }

        public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            await _authenticationService.Login(request.Username, request.Password);

            var user = await _userRepo.GetByEmail(request.Username);
            var roles = await _userRoleRepo.Roles(request.Username);
            
            if (request.AppId.HasValue)
            {
                if (!roles.Any(x => x.ApplicationId == request.AppId))
                    throw new UnauthorizedAccessException();

                // Apply roles for the specific application
                roles = roles.Where(x => x.ApplicationId == request.AppId);
            }
            else
            {
                // Check root access
                if (!roles.Any(x => x.Application.Name == "root"))
                    throw new UnauthorizedAccessException();

                // Apply roles for root
                roles = roles.Where(x => x.Application.Name == "root");
            }

            var claims = await _userRepo.GetClaims(new Guid(user.Id));

            // TODO: Generate access token
            var token = string.Empty;

            return token;
        }
    }
}
