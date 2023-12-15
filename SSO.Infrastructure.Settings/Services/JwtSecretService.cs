namespace SSO.Infrastructure.Settings.Services
{
    public class JwtSecretService
    {
        public string Secret { get; }

        public JwtSecretService(string secret) => Secret = secret;
    }
}
