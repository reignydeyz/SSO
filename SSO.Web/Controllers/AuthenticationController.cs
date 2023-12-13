using MediatR;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.Authentication.Queries;
using SSO.Web.Filters;
using System.IdentityModel.Tokens.Jwt;

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
        public async Task<IActionResult> Init([FromQuery] InitLoginQuery form)
        {
            try
            {
                await _mediator.Send(form);

                if (Request.Cookies["token"] != null)
                {
                    var token = Request.Cookies["token"];
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                    return Redirect($"{form.CallbackUrl}?token=");
                }

                Response.Cookies.Append("appId", form.AppId!.Value.ToString(), new CookieOptions { Expires = DateTime.Now.AddDays(1), HttpOnly = false });

                return Redirect($"{Request.Scheme}://{Request.Host}?appId={form.AppId}&callbackUrl={form.CallbackUrl}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
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

                Response.Cookies.Append("token", res.AccessToken, new CookieOptions { Expires = res.Expires, HttpOnly = false });

                return Ok(res);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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

                Response.Cookies.Append("root", res.AccessToken, new CookieOptions { Expires = res.Expires, HttpOnly = false });

                return Ok(res);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
