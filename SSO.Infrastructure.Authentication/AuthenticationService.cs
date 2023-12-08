using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Models;
using SSO.Infrastructure.Settings.Options;
using System.Security.Claims;
using System.Text;

namespace SSO.Infrastructure.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtOptions _jwtOptions;

        public AuthenticationService(SignInManager<ApplicationUser> signInManager, IOptions<JwtOptions> jwtOptions)
        {
            _signInManager = signInManager;
            _jwtOptions = jwtOptions.Value;
        }

        public Task<string> GenerateAccessToken(Guid applicationId, Guid userId)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Secret));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };

            // TODO: Get roles and claims
            return null;
        }

        public Task<string> GenerateAccessToken(Guid applicationId, string username)
        {
            throw new NotImplementedException();
        }

        public async Task Login(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

            if (!result.Succeeded)
                throw new UnauthorizedAccessException();
        }

        public Task LoginRequest(string applicationId, string applicationSecret, string callbackUrl)
        {
            throw new NotImplementedException();
        }
    }
}
