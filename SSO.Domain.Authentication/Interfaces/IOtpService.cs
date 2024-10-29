namespace SSO.Domain.Authentication.Interfaces
{
    public interface IOtpService
    {
        string GenerateSecretKey();

        bool VerifyOtp(string secretKey, string otp);
    }
}
