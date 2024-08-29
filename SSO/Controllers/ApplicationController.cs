using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.EntityFrameworkCore;
using SSO.Business.Applications;
using SSO.Business.Applications.Commands;
using SSO.Business.Applications.Queries;
using SSO.Filters;
using System.Security.Claims;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        readonly IMediator _mediator;

        public ApplicationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Finds apps
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "RealmAccessPolicy")]
        [EnableQuery(MaxTop = 1000)]
        public IQueryable<ApplicationDto> Get()
        {
            var realmId = new Guid(User.Claims.First(x => x.Type == "realm").Value);

            var res = _mediator.Send(new GetApplicationsQuery { RealmId = realmId }).Result;

            if (Request.Path.HasValue && Request.Path.Value.Contains("/odata"))
                return res;

            return res.Take(1000);
        }

        /// <summary>
        /// Gets app by Id
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("{appId}")]
        [ProducesResponseType(typeof(ApplicationDetailDto), 200)]
        [ODataIgnored]
        [AppIdValidator<GetAppByIdQuery>(ParameterName = "param", PropertyName = "AppId")]
        public async Task<IActionResult> Get([FromRoute] GetAppByIdQuery param)
        {
            try
            {
                var res = await _mediator.Send(param);

                return Ok(res);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Creates new app
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        [ProducesResponseType(typeof(ApplicationDto), 200)]
        [Authorize(Policy = "RealmAccessPolicy")]
        public async Task<IActionResult> Create([FromBody] CreateAppCommand param)
        {
            try
            {
                param.RealmId = new Guid(User.Claims.First(x => x.Type == "realm").Value);
                param.Author = User.Claims.First(x => x.Type == ClaimTypes.GivenName).Value;

                var res = await _mediator.Send(param);

                return Ok(res);
            }
            catch (DbUpdateException)
            {
                return Conflict();
            }
        }

        /// <summary>
        /// Updates app
        /// </summary>
        /// <param name="form"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("{applicationId}")]
        [AppIdValidator<ApplicationIdDto>(Relevant = false)]
        [Authorize(Policy = "RealmAccessPolicy")]
        public async Task<IActionResult> Update([FromRoute] ApplicationIdDto form, [FromBody] UpdateAppCommand param)
        {
            try
            {
                param.Author = User.Claims.First(x => x.Type == ClaimTypes.GivenName).Value;
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
        /// Removes app
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpDelete("{applicationId}")]
        [AppIdValidator<ApplicationIdDto>]
        [Authorize(Policy = "RealmAccessPolicy")]
        public async Task<IActionResult> Delete([FromRoute] ApplicationIdDto form)
        {
            try
            {
                var param = new RemoveAppCommand { ApplicationId = form.ApplicationId!.Value };

                var res = await _mediator.Send(param);

                return Ok();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Creates copy of app
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost("{applicationId}")]
        [AppIdValidator<ApplicationIdDto>]
        [Authorize(Policy = "RealmAccessPolicy")]
        public async Task<IActionResult> Copy([FromRoute] ApplicationIdDto form, [FromBody] CopyAppCommand param)
        {
            try
            {
                param.ApplicationId = form.ApplicationId.Value;
                param.Author = User.Claims.First(x => x.Type == ClaimTypes.GivenName).Value;

                var res = await _mediator.Send(param);

                return Ok(res);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }
    }
}
