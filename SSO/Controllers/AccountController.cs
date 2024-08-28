using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.Accounts;
using SSO.Business.Accounts.Commands;
using SSO.Business.UserApplications.Queries;
using SSO.Business.Users.Queries;
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
        readonly IMapper _mapper;
        readonly IMediator _mediator;

        public AccountController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
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
            var user = await _mediator.Send(new GetUserByIdQuery { UserId = userId! });
            var apps = await _mediator.Send(new GetUserAppsQuery { UserId = new Guid(userId!) });

            var res = _mapper.Map<AccountDto>(user);
            res.Apps = apps.ToList();

            return Ok(res);
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

                param.RealmId = new Guid(User.Claims.First(x => x.Type == ClaimTypes.PrimaryGroupSid).Value);
                param.User = new ApplicationUser { Id = userId! };

                await _mediator.Send(param);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
