using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.ApplicationPermissions;
using SSO.Business.ApplicationPermissions.Queries;
using SSO.Business.Applications;
using SSO.Filters;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/application/{applicationId}/permission")]
    [ApiController]
    [AppIdValidator]
    [Authorize(Policy = "RootPolicy")]
    public class ApplicationPermissionController : ControllerBase
    {
        readonly IMediator _mediator;

        public ApplicationPermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets app`s permissions 
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AppPermissionDto>), 200)]
        public async Task<IActionResult> Get([FromRoute] ApplicationIdDto form)
        {
            var param = new GetAppPermissionsByAppIdQuery { ApplicationId = form.ApplicationId!.Value };

            var res = await _mediator.Send(param);

            return Ok(res);
        }
    }
}