using Microsoft.AspNetCore.Mvc;
using SSO.Business.Users;
using MediatR;
using Microsoft.AspNetCore.OData.Query;
using SSO.Business.ApplicationUsers.Queries;
using Microsoft.AspNetCore.Authorization;

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
    }
}
