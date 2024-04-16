using Microsoft.IdentityModel.Tokens;
using SSO.Domain.Authentication.Interfaces;
using SSO.Infrastructure.Settings.Constants;
using SSO.Infrastructure.Settings.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace SSO.Infrastructure.Authentication
{
    public class TokenService : ITokenService
    {
        readonly RSA _privateKey;

        public TokenService(JwtSecretService jwtSecretService)
        {
            _privateKey = jwtSecretService.PrivateKey;
        }

        public string GenerateToken(ClaimsIdentity claims, DateTime? expiry = null, string? issuer = null, string? audience = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiry ?? DateTime.Now.AddDays(1),
                Issuer = issuer ?? TokenValidationParamConstants.Issuer,
                Audience = audience ?? issuer ?? TokenValidationParamConstants.Audience,
                SigningCredentials = new SigningCredentials(new RsaSecurityKey(_privateKey), SecurityAlgorithms.RsaSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
