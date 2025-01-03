using MediatR;
using Microsoft.Extensions.Caching.Memory;
using SSO.Business.Authentication.Queries;
using System.IdentityModel.Tokens.Jwt;

namespace SSO.Business.Authentication.Handlers
{
    public class GetRequestTokenQueryHandler : IRequestHandler<GetAccessTokenQuery, TokenDto>
    {
        readonly IMemoryCache _cache;

        public GetRequestTokenQueryHandler(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<TokenDto> Handle(GetAccessTokenQuery request, CancellationToken cancellationToken)
        {
            var token = _cache.Get<string?>(request.RequestToken.ToString());

            if (token is null)
                throw new UnauthorizedAccessException("Invalid token.");

            return new TokenDto
            {
                Id = request.RequestToken,
                AccessToken = token,
                Expires = GetJwtExpiryDateTime(token)
            };
        }

        private static DateTime GetJwtExpiryDateTime(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();

            // Parse the JWT token
            var jsonToken = handler.ReadToken(jwt) as JwtSecurityToken;

            if (jsonToken == null)
            {
                throw new ArgumentException("Invalid JWT format.");
            }

            // Get the 'exp' claim which is the expiration time
            var expClaim = jsonToken.Payload.Exp;

            if (!expClaim.HasValue)
            {
                throw new ArgumentException("The JWT does not contain an 'exp' claim.");
            }

            // Convert the Unix timestamp to DateTime
            DateTime expiryDate = DateTimeOffset.FromUnixTimeSeconds(expClaim.Value).UtcDateTime;

            return expiryDate;
        }
    }
}
