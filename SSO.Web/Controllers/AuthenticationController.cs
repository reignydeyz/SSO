using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.Authentication.Queries;
using SSO.Web.Filters;

namespace SSO.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AppIdValidator]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Request for login page
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get([FromQuery] InitLoginQuery form)
        {
            return Redirect($"{Request.Scheme}://{Request.Host}?appId={form.AppId}&callbackUrl={form.CallbackUrl}");
        }

        /// <summary>
        /// Gets access token
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> Post([FromBody] LoginQuery form)
        {
            try
            {
                var res = await _mediator.Send(form);

                return Ok(res);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}
