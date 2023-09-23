using MediatR;
using SSO.Business.Authentication.Queries;
using SSO.Domain.Interfaces;

namespace SSO.Business.Authentication.Handlers
{
    /// <summary>
    /// Handler for login request
    /// </summary>
    public class LoginQueryHandler : IRequestHandler<LoginQuery, string>
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginQueryHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            await _authenticationService.Login(request.Username, request.Password);

            // TODO: Generate access token
            var token = string.Empty;

            return token;
        }
    }
}
