using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Business.Groups;
using SSO.Business.Groups.Commands;
using SSO.Filters;
using SSO.Infrastructure.Settings.Enums;
using System.Security.Claims;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RootPolicy")]
    [RealmValidator(Realm = Realm.Default)]
    public class GroupController : ControllerBase
    {
        readonly IMediator _mediator;

        public GroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates new group
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(GroupDto), 200)]
        public async Task<IActionResult> Create([FromBody] CreateGroupCommand param)
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

        /// <summary>
        /// Removes group
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpDelete("{groupId}")]
        [GroupIdValidator]
        public async Task<IActionResult> Delete([FromRoute] GroupIdDto form)
        {
            try
            {
                var param = new RemoveGroupCommand { GroupId = form.GroupId };

                var res = await _mediator.Send(param);

                return Ok();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Updates group
        /// </summary>
        /// <param name="form"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("{groupId}")]
        [GroupIdValidator]
        public async Task<IActionResult> Update([FromRoute] GroupIdDto form, [FromBody] UpdateGroupCommand param)
        {
            try
            {
                param.Author = User.Claims.First(x => x.Type == ClaimTypes.GivenName).Value;
                param.GroupId = form.GroupId;

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
