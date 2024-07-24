using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Business.ApplicationCallbacks;
using SSO.Business.ApplicationCallbacks.Commands;
using SSO.Business.ApplicationCallbacks.Queries;
using SSO.Business.Applications;
using SSO.Filters;
using System.ComponentModel.DataAnnotations;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/application/{applicationId}/callback")]
    [ApiController]
    [AppIdValidator<ApplicationIdDto>]
    [Authorize(Policy = "RealmAccessPolicy")]
    public class ApplicationCallbackController : ControllerBase
    {
        readonly IMediator _mediator;

        public ApplicationCallbackController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets app`s callbacks
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        public async Task<IActionResult> Get([FromRoute] ApplicationIdDto form)
        {
            var param = new GetAppCallbacksByAppIdQuery { ApplicationId = form.ApplicationId!.Value };

            var res = await _mediator.Send(param);

            return Ok(res);
        }

        /// <summary>
        /// Creates new app callback
        /// </summary>
        /// <param name="form"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(AppCallbackDto), 200)]
        public async Task<IActionResult> Create([FromRoute] ApplicationIdDto form, [FromBody] CreateAppCallbackCommand param)
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
        /// Removes app`s callback
        /// </summary>
        /// <param name="form"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] ApplicationIdDto form, [FromQuery, Url, Required] string url)
        {
            try
            {
                var param = new RemoveAppCallbackCommand { ApplicationId = form.ApplicationId!.Value, Url = url };

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
