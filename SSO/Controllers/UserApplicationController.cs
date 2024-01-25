using MediatR;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.Applications;
using SSO.Business.Users;
using SSO.Filters;
using SSO.Business.UserApplications.Queries;
using Microsoft.AspNetCore.Authorization;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/user/{userId}/application")]
    [ApiController]
    [Authorize(Policy = "RootPolicy")]
    public class UserApplicationController : ControllerBase
    {
        readonly IMediator _mediator;

        public UserApplicationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets user`s assigned apps 
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpGet]
        [UserIdValidator]
        [ProducesResponseType(typeof(IEnumerable<ApplicationDto>), 200)]
        public async Task<IActionResult> Get([FromRoute] UserIdDto form)
        {
            var param = new GetUserAppsQuery { UserId = form.UserId!.Value };

            var res = await _mediator.Send(param);

            return Ok(res);
        }
    }
}
