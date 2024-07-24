using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.ApplicationGroupRoles;
using SSO.Business.ApplicationGroupRoles.Commands;
using SSO.Business.ApplicationGroupRoles.Queries;
using SSO.Business.ApplicationRoles;
using SSO.Filters;
using System.ComponentModel.DataAnnotations;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/application/{applicationId}/group/{groupId}/role")]
    [ApiController]    
    [Authorize(Policy = "RealmAccessPolicy")]
    [GroupIdValidator<AppGroupIdDto>]
    public class ApplicationGroupRoleController : ControllerBase
    {
        readonly IMediator _mediator;

        public ApplicationGroupRoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets app group`s roles
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AppRoleDto>), 200)]
        public async Task<IActionResult> Get([FromRoute] AppGroupIdDto form)
        {
            var param = new GetAppGroupRolesQuery { ApplicationId = form.ApplicationId, GroupId = form.GroupId };

            var res = await _mediator.Send(param);

            return Ok(res);
        }

        /// <summary>
        /// Updates app group`s roles
        /// </summary>
        /// <param name="form"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromRoute] AppGroupIdDto form, [FromBody, Required, MinLength(1)] List<Guid> roleIds)
        {
            var param = new UpdateGroupRolesCommand { ApplicationId = form.ApplicationId, GroupId = form.GroupId, RoleIds = roleIds };

            await _mediator.Send(param);

            return Ok();
        }
    }
}
