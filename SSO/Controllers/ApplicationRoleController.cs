using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.ApplicationRoles.Queries;
using SSO.Business.ApplicationRoles;
using SSO.Business.Applications;
using SSO.Filters;
using Microsoft.EntityFrameworkCore;
using SSO.Business.ApplicationRoles.Commands;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/application/{applicationId}/role")]
    [ApiController]
    [AppIdValidator<ApplicationIdDto>]
    [Authorize(Policy = "RealmAccessPolicy")]
    public class ApplicationRoleController : ControllerBase
    {
        readonly IMediator _mediator;

        public ApplicationRoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets app`s roles 
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AppRoleDto>), 200)]
        public async Task<IActionResult> Get([FromRoute] ApplicationIdDto form)
        {
            var param = new GetAppRolesByAppIdQuery { ApplicationId = form.ApplicationId!.Value };

            var res = await _mediator.Send(param);

            return Ok(res);
        }

        /// <summary>
        /// Creates new app role
        /// </summary>
        /// <param name="form"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromRoute] ApplicationIdDto form, [FromBody] CreateAppRoleCommand param)
        {
            try
            {
                param.ApplicationId = form.ApplicationId!.Value;

                var res = await _mediator.Send(param);

                return Ok(res);
            }
            catch (DbUpdateException ex)
            {
                return Conflict();
            }
        }

        /// <summary>
        /// Removes app role
        /// </summary>
        /// <param name="form"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] ApplicationIdDto form, [FromRoute] Guid id)
        {
            try
            {
                var param = new RemoveAppRoleCommand { ApplicationId = form.ApplicationId!.Value, RoleId = id };

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
