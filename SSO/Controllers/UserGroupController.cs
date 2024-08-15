using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.Groups;
using SSO.Business.UserGroups.Queries;
using SSO.Business.Users;
using SSO.Filters;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/user/{userId}/group")]
    [ApiController]
    [Authorize(Policy = "RealmAccessPolicy")]
    public class UserGroupController : ControllerBase
    {
        readonly IMediator _mediator;

        public UserGroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets user`s groups
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpGet]
        [UserIdValidator]
        [ProducesResponseType(typeof(IEnumerable<GroupDto>), 200)]
        public async Task<IActionResult> Get([FromRoute] UserIdDto form)
        {
            var param = new GetUserGroupsQuery { UserId = form.UserId!.Value };

            var res = await _mediator.Send(param);

            return Ok(res);
        }
    }
}
