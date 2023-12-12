using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SSO.Domain.Authentication.Interfaces;
using SSO.Infrastructure.Settings.Constants;
using SSO.Infrastructure.Settings.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SSO.Infrastructure.Authentication
{
    public class TokenService : ITokenService
    {

        private readonly JwtOptions _jwtOptions;

        public TokenService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateToken(ClaimsIdentity claims, DateTime? expiry = null)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Secret));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiry ?? DateTime.Now.AddDays(1),
                Issuer = TokenValidationParamConstants.Issuer,
                Audience = TokenValidationParamConstants.Audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
