using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.EntityFrameworkCore;
using SSO.Business.Applications;
using SSO.Business.Applications.Commands;
using SSO.Business.Applications.Queries;
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
        [Authorize(Policy = "RootPolicy")]
        [EnableQuery]
        public IQueryable<ApplicationDto> Get()
        {
            return _mediator.Send(new GetApplicationsQuery { }).Result;
        }

        /// <summary>
        /// Gets app by Id
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("{appId}")]
        [ProducesResponseType(typeof(ApplicationDetailDto), 200)]
        [ODataIgnored]
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
        [Authorize(Policy = "RootPolicy")]
        public async Task<IActionResult> Create([FromBody] CreateAppCommand param)
        {
            try
            {
                param.Author = User.Claims.First(x => x.Type == ClaimTypes.GivenName).Value;

                var res = await _mediator.Send(param);

                return Ok(res);
            }
            catch (DbUpdateException)
            {
                return Conflict();
            }
        }
    }
}
