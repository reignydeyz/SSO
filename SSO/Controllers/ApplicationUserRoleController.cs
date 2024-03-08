using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.ApplicationRoles;
using SSO.Business.ApplicationUserRoles;
using SSO.Business.ApplicationUserRoles.Commands;
using SSO.Business.ApplicationUserRoles.Queries;
using SSO.Filters;
using System.ComponentModel.DataAnnotations;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/application/{applicationId}/user/{userId}/role")]
    [ApiController]
    [AppUserIdValidator]
    [Authorize(Policy = "RootPolicy")]
    public class ApplicationUserRoleController : ControllerBase
    {
        readonly IMediator _mediator;

        public ApplicationUserRoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets app user`s roles
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AppRoleDto>), 200)]
        public async Task<IActionResult> Get([FromRoute] AppUserIdDto form)
        {
            var param = new GetAppUserRolesQuery { ApplicationId = form.ApplicationId, UserId = form.UserId };

            var res = await _mediator.Send(param);

            return Ok(res);
        }

        /// <summary>
        /// Updates app user`s roles
        /// </summary>
        /// <param name="form"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromRoute] AppUserIdDto form, [FromBody, Required, MinLength(1)] List<Guid> roleIds)
        {
            var param = new UpdateAppUserRolesCommand { ApplicationId = form.ApplicationId, UserId = form.UserId, RoleIds = roleIds };

            await _mediator.Send(param);

            return Ok();
        }
    }
}
