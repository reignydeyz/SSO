using System.Security.Claims;

namespace SSO.Domain.Authentication.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(ClaimsIdentity claims, DateTime? expiry = null, string? issuer = null, string? audience = null);
    }
}
