using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using SSO.Business.Users;
using SSO.Business.Users.Commands;
using SSO.Business.Users.Queries;
using SSO.Filters;
using System.Security.Claims;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RootPolicy")]
    public class UserController : ControllerBase
    {
        readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Finds users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery(MaxTop = 1000)]
        public IQueryable<UserDto> Get()
        {
            var realmId = new Guid(User.Claims.First(x => x.Type == ClaimTypes.PrimaryGroupSid).Value);
            var res = _mediator.Send(new GetUsersQuery { RealmId = realmId }).Result;

            if (Request.Path.HasValue && Request.Path.Value.Contains("/odata"))
                return res;

            return res.Take(1000);
        }

        /// <summary>
        /// Gets user by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var realmId = new Guid(User.Claims.First(x => x.Type == ClaimTypes.PrimaryGroupSid).Value);
                var res = await _mediator.Send(new GetUserByIdQuery { UserId = id.ToString(), RealmId = realmId });

                return Ok(res);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Creates new user
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), 200)]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand param)
        {
            try
            {
                param.RealmId = new Guid(User.Claims.First(x => x.Type == ClaimTypes.PrimaryGroupSid).Value);
                param.Author = User.Claims.First(x => x.Type == ClaimTypes.GivenName).Value;

                var res = await _mediator.Send(param);

                return Ok(res);
            }
            catch (DbUpdateException)
            {
                return Conflict();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates user
        /// </summary>
        /// <param name="form"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        [UserIdValidator(Relevant = false)]
        [ProducesResponseType(typeof(UserDto), 200)]
        public async Task<IActionResult> Update([FromRoute] UserIdDto form, [FromBody] UpdateUserCommand param)
        {
            try
            {
                param.UserId = form.UserId.ToString();
                param.Author = User.Claims.First(x => x.Type == ClaimTypes.GivenName).Value;

                var res = await _mediator.Send(param);

                return Ok(res);
            }
            catch (DbUpdateException)
            {
                return Conflict();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Copies user
        /// </summary>
        /// <param name="form"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("{userId}")]
        [UserIdValidator(Relevant = false)]
        [ProducesResponseType(typeof(UserDto), 200)]
        public async Task<IActionResult> Copy([FromRoute] UserIdDto form, [FromBody] CopyUserCommand param)
        {
            try
            {
                param.RealmId = new Guid(User.Claims.First(x => x.Type == ClaimTypes.PrimaryGroupSid).Value);
                param.UserId = form.UserId.ToString();
                param.Author = User.Claims.First(x => x.Type == ClaimTypes.GivenName).Value;

                var res = await _mediator.Send(param);

                return Ok(res);
            }
            catch (DbUpdateException)
            {
                return Conflict();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Removes user
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        [UserIdValidator]
        public async Task<IActionResult> Delete([FromRoute] UserIdDto form)
        {
            var realmId = new Guid(User.Claims.First(x => x.Type == ClaimTypes.PrimaryGroupSid).Value);
            var param = new RemoveUserCommand { UserId = form.UserId.ToString(), RealmId = realmId };

            var res = await _mediator.Send(param);

            return Ok();
        }
    }
}
