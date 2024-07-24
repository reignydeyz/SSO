using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.Applications;
using SSO.Business.UserApplications.Queries;
using SSO.Business.Users;
using SSO.Filters;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/user/{userId}/application")]
    [ApiController]
    [Authorize(Policy = "RealmAccessPolicy")]
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
