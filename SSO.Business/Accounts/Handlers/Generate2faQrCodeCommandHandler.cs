using MediatR;
using Microsoft.AspNetCore.Identity;
using QRCoder;
using SSO.Business.Accounts.Commands;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Models;
using SSO.Infrastructure.Helpers;
using SSO.Infrastructure.Settings.Services;
using System.Security.Cryptography;
using System.Text;

namespace SSO.Business.Accounts.Handlers
{
    public class Generate2faQrCodeCommandHandler : IRequestHandler<Generate2faQrCodeCommand, string>
    {
        readonly JwtSecretService _jwtSecretService;
        readonly RsaKeyService _rsaKeyService;
        readonly UserManager<ApplicationUser> _userManager;
        readonly IOtpService _otpService;

        public Generate2faQrCodeCommandHandler(JwtSecretService jwtSecretService, RsaKeyService rsaKeyService, UserManager<ApplicationUser> userManager, IOtpService otpService)
        {
            _rsaKeyService = rsaKeyService;
            _jwtSecretService = jwtSecretService;
            _userManager = userManager;
            _otpService = otpService;
        }

        public async Task<string> Handle(Generate2faQrCodeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.User!.Id.ToString());

            var publicKey = RSA.Create(); publicKey.ImportParameters(_jwtSecretService.PrivateKey.ExportParameters(false));
            var secret = _otpService.GenerateSecretKey();
            var encSecret = _rsaKeyService.EncryptString(secret, publicKey);

            if (!user.TwoFactorEnabled)
            {
                var key = CryptographyHelper.GenerateKey();

                user.TwoFactorEnabled = true;
                user.TwoFactorSecret = Encoding.ASCII.GetBytes(encSecret);

                await _userManager.UpdateAsync(user);
            }

            var otpAuthUrl = $"otpauth://totp/SSO:{user.UserName}?secret={secret}&issuer=SSO&digits=6&period=30";

            return GenerateQrCodeImage(otpAuthUrl);
        }

        private string GenerateQrCodeImage(string otpAuthUri)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(otpAuthUri, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new PngByteQRCode(qrCodeData);
                var qrCodeBytes = qrCode.GetGraphic(20);  // 20 is the pixel density of the image
                return Convert.ToBase64String(qrCodeBytes);  // Return as base64-encoded string
            }
        }
    }
}
