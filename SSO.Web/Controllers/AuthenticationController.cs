using MediatR;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.Authentication.Queries;
using SSO.Web.Filters;

namespace SSO.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [AppIdValidator]
        public IActionResult Init([FromQuery] InitLoginQuery form)
        {
            return Redirect($"{Request.Scheme}://{Request.Host}?appId={form.AppId}&callbackUrl={form.CallbackUrl}");
        }

        /// <summary>
        /// Gets access token
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [AppIdValidator]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> Login([FromBody] LoginQuery form)
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

        /// <summary>
        /// Gets access token as root
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost("root")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> LoginAsRoot([FromBody] LoginAsRootQuery form)
        {
            try
            {
                var res = await _mediator.Send(form);

                return Ok(res);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}
