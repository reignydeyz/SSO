using MediatR;
using Microsoft.AspNetCore.Identity;
using QRCoder;
using SSO.Business.Accounts.Commands;
using SSO.Domain.Authentication.Interfaces;
using SSO.Domain.Models;

namespace SSO.Business.Accounts.Handlers
{
    public class Generate2faQrCodeCommandHandler : IRequestHandler<Generate2faQrCodeCommand, string>
    {
        readonly UserManager<ApplicationUser> _userManager;
        readonly IOtpService _otpService;

        public Generate2faQrCodeCommandHandler(UserManager<ApplicationUser> userManager, IOtpService otpService)
        {
            _userManager = userManager;
            _otpService = otpService;
        }

        public async Task<string> Handle(Generate2faQrCodeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.User!.Id.ToString());
            
            if (!user.TwoFactorEnabled)
            {
                user.TwoFactorEnabled = true;
                user.TwoFactorSecretKey = _otpService.GenerateSecretKey();

                await _userManager.UpdateAsync(user);
            }

            var otpAuthUrl = $"otpauth://totp/SSO:{user.UserName}?secret={user.TwoFactorSecretKey}&issuer=SSO&digits=6&period=30";

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
