using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.Applications;
using SSO.Business.ApplicationUsers.Queries;
using System.Security.Claims;

namespace SSO.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets apps tied to authenticated user
        /// </summary>
        /// <returns></returns>
        [HttpGet("apps")]
        [ProducesResponseType(typeof(IList<ApplicationDto>), 200)]
        public async Task<IActionResult> GetApps()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var res = await _mediator.Send(new GetAppsByUserIdQuery { UserId = new Guid(userId) });

            return Ok(res);
        }
    }
}
