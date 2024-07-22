using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.Accounts.Commands;
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
        readonly IMapper _mapper;

        public AccountController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
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

                param.RealmId = new Guid(User.Claims.First(x => x.Type == ClaimTypes.System).Value);
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
