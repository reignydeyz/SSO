using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using SSO.Business.ApplicationUserRoles;
using SSO.Business.ApplicationUsers.Commands;
using SSO.Business.ApplicationUsers.Queries;
using SSO.Business.Users;
using SSO.Filters;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/application/{applicationId}/user")]
    [ApiController]
    [Authorize(Policy = "RootPolicy")]
    public class ApplicationUserController : ControllerBase
    {
        readonly IMediator _mediator;

        public ApplicationUserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets app`s users 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery(MaxTop = 1000)]
        [ProducesResponseType(typeof(List<UserDto>), 200)]
        public async Task<IActionResult> Get([FromRoute] Guid applicationId, [FromQuery] Guid? appId = null)
        {
            try
            {
                if (Request.Path.HasValue && Request.Path.Value.Contains("/odata"))
                {
                    if (appId is null)
                        throw new ArgumentNullException();

                    var res = _mediator.Send(new GetUsersByApplicationIdQuery { ApplicationId = appId!.Value }).Result;

                    return Ok(res);
                }

                var res1 = _mediator.Send(new GetUsersByApplicationIdQuery { ApplicationId = applicationId }).Result.Take(1000);

                return Ok(res1);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Removes user from app
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        [AppUserIdValidator]
        public async Task<IActionResult> Delete([FromRoute] AppUserIdDto form)
        {
            var param = new RemoveAppUserCommand { UserId = form.UserId, ApplicationId = form.ApplicationId };

            await _mediator.Send(param);

            return Ok();
        }
    }
}
