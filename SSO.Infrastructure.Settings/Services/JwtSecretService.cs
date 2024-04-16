using System.Security.Cryptography;

namespace SSO.Infrastructure.Settings.Services
{
    public class JwtSecretService
    {
        public RSA PrivateKey { get; }

        public JwtSecretService(RSA privateKey) => PrivateKey = privateKey;
    }
}
