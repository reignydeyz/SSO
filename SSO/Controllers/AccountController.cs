using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.Accounts;
using SSO.Business.Accounts.Commands;
using SSO.Business.Accounts.Queries;
using SSO.Business.UserApplications.Queries;
using SSO.Domain.Models;
using System.Security.Claims;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets account details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(AccountDto), 200)]
        public async Task<IActionResult> Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var account = await _mediator.Send(new GetAccountByUserIdQuery { UserId = userId! });
            var apps = await _mediator.Send(new GetUserAppsQuery { UserId = new Guid(userId!) });

            account.Apps = apps.ToList();

            return Ok(account);
        }

        /// <summary>
        /// Changes password
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand param)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                param.RealmId = new Guid(User.Claims.First(x => x.Type == "realm").Value);
                param.User = new ApplicationUser { Id = userId! };

                await _mediator.Send(param);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Generates 2FA QR code
        /// </summary>
        /// <returns></returns>
        [HttpPost("2fa")]
        public async Task<IActionResult> Generate2faQrCode()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var res = await _mediator.Send(new Generate2faQrCodeCommand { User = new ApplicationUser { Id = userId } });
            var imageBytes = Convert.FromBase64String(res);

            return File(imageBytes, "image/png");
        }

        /// <summary>
        /// Disable 2FA
        /// </summary>
        /// <returns></returns>
        [HttpDelete("2fa")]
        public async Task<IActionResult> Disable2fa()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var res = await _mediator.Send(new Disable2faCommand { User = new ApplicationUser { Id = userId } });

            return Ok();
        }
    }
}
