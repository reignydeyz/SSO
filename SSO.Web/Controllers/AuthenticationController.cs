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
        /// <param name="callbackUrl"></param>
        /// <returns></returns>
        [ValidateOrigin]
        [HttpGet]
        public IActionResult Get([FromQuery] string callbackUrl)
        {
            // TODO: Redirect to login page
            return Ok();
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
