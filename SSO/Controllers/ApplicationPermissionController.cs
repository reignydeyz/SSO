using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Business.ApplicationPermissions;
using SSO.Business.ApplicationPermissions.Commands;
using SSO.Business.ApplicationPermissions.Queries;
using SSO.Business.Applications;
using SSO.Filters;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/application/{applicationId}/permission")]
    [ApiController]
    [AppIdValidator<ApplicationIdDto>]
    [Authorize(Policy = "RealmAccessPolicy")]
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

        /// <summary>
        /// Creates new app permission
        /// </summary>
        /// <param name="form"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromRoute] ApplicationIdDto form, [FromBody] CreateAppPermissionCommand param)
        {
            try
            {
                param.ApplicationId = form.ApplicationId!.Value;

                var res = await _mediator.Send(param);

                return Ok(res);
            }
            catch (DbUpdateException)
            {
                return Conflict();
            }
        }

        /// <summary>
        /// Removes app's permission
        /// </summary>
        /// <param name="form"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] ApplicationIdDto form, [FromRoute] Guid id)
        {
            try
            {
                var param = new RemoveAppPermissionCommand { ApplicationId = form.ApplicationId!.Value, PermissionId = id };

                var res = await _mediator.Send(param);

                return Ok();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }
    }
}