using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.Applications;
using SSO.Business.GroupApplications.Queries;
using SSO.Business.Groups;
using SSO.Filters;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/group/{groupId}/application")]
    [ApiController]
    [Authorize(Policy = "RealmAccessPolicy")]
    public class GroupApplicationController : ControllerBase
    {
        readonly IMediator _mediator;

        public GroupApplicationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets group`s assigned apps 
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpGet]
        [GroupIdValidator<GroupIdDto>]
        [ProducesResponseType(typeof(IEnumerable<ApplicationDto>), 200)]
        public async Task<IActionResult> Get([FromRoute] GroupIdDto form)
        {
            var param = new GetGroupAppsQuery { GroupId = form.GroupId };

            var res = await _mediator.Send(param);

            return Ok(res);
        }
    }
}
