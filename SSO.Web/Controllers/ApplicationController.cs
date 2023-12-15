using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Business.Applications;
using SSO.Business.Applications.Commands;
using System.Security.Claims;

namespace SSO.Web.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Authorize(Policy = "RootPolicy")]
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
        /// Creates new app
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        [ProducesResponseType(typeof(ApplicationDto), 200)]
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
