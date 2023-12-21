using Microsoft.IdentityModel.Tokens;
using SSO.Domain.Authentication.Interfaces;
using SSO.Infrastructure.Settings.Constants;
using SSO.Infrastructure.Settings.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SSO.Infrastructure.Authentication
{
    public class TokenService : ITokenService
    {
        readonly string _jwtSecret;

        public TokenService(JwtSecretService jwtSecretService)
        {
            _jwtSecret = jwtSecretService.Secret;
        }

        public string GenerateToken(ClaimsIdentity claims, DateTime? expiry = null, string? issuer = null, string? audience = null)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSecret));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiry ?? DateTime.Now.AddDays(1),
                Issuer = issuer ?? TokenValidationParamConstants.Issuer,
                Audience = audience ?? issuer ?? TokenValidationParamConstants.Audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
