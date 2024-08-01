using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.RealmIdpSettings.Commands;
using SSO.Business.RealmIdpSettings.Queries;
using SSO.Infrastructure.LDAP.Models;
using System.Security.Claims;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/realm/idp")]
    [ApiController]
    [Authorize(Policy = "RealmAccessPolicy")]
    public class RealmIdpSettingsController : ControllerBase
    {
        readonly IMediator _mediator;

        public RealmIdpSettingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates or updates LDAP settings
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("ldap")]
        public async Task<IActionResult> CreateOrUpdateLdapSettings([FromBody] LDAPSettings param)
        {
            var cmd = new ModifyRealmLdapSettingsCommand
            {
                RealmId = new Guid(User.Claims.First(x => x.Type == ClaimTypes.PrimaryGroupSid).Value),
                LDAPSettings = param
            };

            await _mediator.Send(cmd);

            return Ok();
        }

        /// <summary>
        /// Tests LDAP settings
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("ldap/test")]
        public async Task<IActionResult> TestLdapSettings([FromBody] TestLdapSettingsQuery param)
        {
            try
            {
                await _mediator.Send(param);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
