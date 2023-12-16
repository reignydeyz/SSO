﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.Authentication.Queries;
using SSO.Filters;

namespace SSO.Controllers
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
        /// Request for app login
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpGet]
        [AppIdValidator]
        [ApiExplorerSettings(GroupName = "Client")]
        public async Task<IActionResult> Init([FromQuery] InitLoginQuery form)
        {
            try
            {
                await _mediator.Send(form);

                Response.Cookies.Append("appId", form.AppId!.Value.ToString(), new CookieOptions { Expires = DateTime.Now.AddDays(1), HttpOnly = false });

                if (Request.Cookies["token"] != null)
                {
                    var token = await _mediator.Send(new SwitchAppQuery { Token = Request.Cookies["token"], AppId = form.AppId });

                    Response.Cookies.Append("token", token.AccessToken, new CookieOptions { Expires = token.Expires, HttpOnly = false });

                    return Redirect($"{form.CallbackUrl}?token={token.AccessToken}");
                }

                return Redirect($"{Request.Scheme}://{Request.Host}/login?appId={form.AppId}&callbackUrl={form.CallbackUrl}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return Redirect($"{Request.Scheme}://{Request.Host}/login?appId={form.AppId}&callbackUrl={form.CallbackUrl}");
            }
        }

        /// <summary>
        /// Authentication for app user
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [AppIdValidator]
        [ProducesResponseType(typeof(string), 200)]
        [ApiExplorerSettings(GroupName = "System")]
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
        /// Authentication for system
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost("system")]
        [ProducesResponseType(typeof(string), 200)]
        [ApiExplorerSettings(GroupName = "System")]
        public async Task<IActionResult> LoginToSystem([FromBody] LoginToSystemQuery form)
        {
            try
            {
                var res = await _mediator.Send(form);

                Response.Cookies.Append("system", res.AccessToken, new CookieOptions { Expires = res.Expires, HttpOnly = false });

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