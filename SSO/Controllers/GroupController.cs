using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.EntityFrameworkCore;
using SSO.Business.Groups;
using SSO.Business.Groups.Commands;
using SSO.Business.Groups.Queries;
using SSO.Filters;
using System.Security.Claims;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RootPolicy")]
    public class GroupController : ControllerBase
    {
        readonly IMediator _mediator;

        public GroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Finds groups
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery(MaxTop = 1000)]
        public IQueryable<GroupDto> Get()
        {
            var res = _mediator.Send(new GetGroupsQuery { }).Result;

            if (Request.Path.HasValue && Request.Path.Value.Contains("/odata"))
                return res;

            return res.Take(1000);
        }

        /// <summary>
        /// Gets group by Id
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("{groupId}")]
        [ProducesResponseType(typeof(GroupDto), 200)]
        [ODataIgnored]
        [GroupIdValidator<GetGroupByIdQuery>(ParameterName = "param")]
        public async Task<IActionResult> Get([FromRoute] GetGroupByIdQuery param)
        {
            try
            {
                var res = await _mediator.Send(param);

                return Ok(res);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
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
        [GroupIdValidator<GroupIdDto>]
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
        [GroupIdValidator<GroupIdDto>]
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
