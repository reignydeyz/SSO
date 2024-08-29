using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.RealmIdpSettings.Commands;
using SSO.Business.RealmIdpSettings.Queries;
using SSO.Infrastructure.LDAP.Models;
using SSO.Infrastructure.Settings.Enums;
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
        readonly IMapper _mapper;

        public RealmIdpSettingsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates or updates LDAP settings
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("ldap")]
        public async Task<IActionResult> CreateOrUpdateLdapSettings([FromBody] LDAPSettings param)
        {
            var qry = _mapper.Map<TestLdapSettingsQuery>(param);
            var test = await TestLdapSettings(qry);

            if (test is not OkResult)
                return test;

            var cmd = new ModifyRealmLdapSettingsCommand
            {
                RealmId = new Guid(User.Claims.First(x => x.Type == "realm").Value),
                LDAPSettings = param
            };

            await _mediator.Send(cmd);

            return Ok();
        }

        /// <summary>
        /// Removes LDAP settings
        /// </summary>
        /// <returns></returns>
        [HttpDelete("ldap")]
        public async Task<IActionResult> RemoveLdapSettings()
        {
            var cmd = new RemoveRealmIdpSettingsCommand
            {
                RealmId = new Guid(User.Claims.First(x => x.Type == "realm").Value),
                IdentityProvider = IdentityProvider.LDAP
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
