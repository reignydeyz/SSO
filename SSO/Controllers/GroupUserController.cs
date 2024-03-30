using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using SSO.Business.GroupUsers.Commands;
using SSO.Business.GroupUsers.Queries;
using SSO.Business.Users;
using SSO.Filters;
using SSO.Infrastructure.Settings.Enums;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/group/{groupId}/user")]
    [ApiController]
    [Authorize(Policy = "RootPolicy")]
    [RealmValidator(Realm = Realm.Default)]
    public class GroupUserController : ControllerBase
    {
        readonly IMediator _mediator;

        public GroupUserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets group`s members
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery(MaxTop = 1000)]
        [ProducesResponseType(typeof(List<UserDto>), 200)]
        public async Task<IActionResult> Get([FromRoute] Guid groupId, [FromQuery] Guid? group)
        {
            try
            {
                if (Request.Path.HasValue && Request.Path.Value.Contains("/odata"))
                {
                    if (group is null)
                        throw new ArgumentNullException();

                    var res = await _mediator.Send(new GetUsersByGroupIdQuery { GroupId = group!.Value });

                    return Ok(res);
                }

                var res1 = _mediator.Send(new GetUsersByGroupIdQuery { GroupId = groupId }).Result.Take(1000);

                return Ok(res1);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Adds user to group
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(OkResult), 200)]
        public async Task<IActionResult> Create([FromRoute] Guid groupId, [FromBody] Guid userId)
        {
            try
            {
                var param = new CreateGroupUserCommand
                {
                    GroupId = groupId,
                    UserId = userId
                };

                var res = await _mediator.Send(param);

                return Ok();
            }
            catch (DbUpdateException)
            {
                return Conflict();
            }
        }

        /// <summary>
        /// Removes user from group
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(Guid groupId, Guid userId)
        {
            var param = new RemoveGroupUserCommand { UserId = userId, GroupId = groupId };

            await _mediator.Send(param);

            return Ok();
        }
    }
}
