using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.Realms;
using SSO.Business.Realms.Queries;
using System.Security.Claims;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RealmAccessPolicy")]
    public class RealmController : ControllerBase
    {
        readonly IMediator _mediator;

        public RealmController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets current realm details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(RealmDto), 200)]
        public async Task<IActionResult> Get()
        {
            var realmId = new Guid(User.Claims.First(x => x.Type == ClaimTypes.PrimaryGroupSid).Value);

            var res = await _mediator.Send(new GetRealmByIdQuery { RealmId = realmId });

            return Ok(res);
        }
    }
}
