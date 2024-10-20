using Microsoft.Extensions.DependencyInjection;
using OtpNet;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Management.Interfaces;

namespace SSO.Infrastructure.Authentication
{
    public class OtpService : IOtpService
    {
        readonly IUserRepository _userRepository;

        public OtpService(IServiceProvider serviceProvider)
        {
            _userRepository = serviceProvider.GetRequiredService<Management.UserRepository>();
        }

        public string GenerateSecretKey()
        {
            // Generate a random 160-bit secret key (Base32 encoded)
            var secretKey = KeyGeneration.GenerateRandomKey(20);
            return Base32Encoding.ToString(secretKey);
        }

        public bool VerifyOtp(string secretKey, string otp)
        {
            var totp = new Totp(Base32Encoding.ToBytes(secretKey));
            return totp.VerifyTotp(otp, out long timeWindow);  // Verifies OTP is valid within the time window
        }
    }
}
