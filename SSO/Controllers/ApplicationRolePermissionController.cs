using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.ApplicationPermissions;
using SSO.Business.ApplicationRolePermissions.Commands;
using SSO.Business.ApplicationRolePermissions.Queries;
using SSO.Business.ApplicationRoles;
using SSO.Filters;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/application/{applicationId}/role/{roleId}/permission")]
    [ApiController]
    [AppRoleIdValidator]
    [Authorize(Policy = "RootPolicy")]
    public class ApplicationRolePermissionController : ControllerBase
    {
        readonly IMediator _mediator;

        public ApplicationRolePermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets app role`s permissions 
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AppPermissionDto>), 200)]
        public async Task<IActionResult> Get([FromRoute] AppRoleIdDto form)
        {
            var param = new GetAppRolePermissionsQuery { ApplicationId = form.ApplicationId, RoleId = form.RoleId };

            var res = await _mediator.Send(param);

            return Ok(res);
        }

        /// <summary>
        /// Updates app role`s permissions
        /// </summary>
        /// <param name="form"></param>
        /// <param name="permissionIds"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromRoute] AppRoleIdDto form, [FromBody, Required, MinLength(1)] List<Guid> permissionIds)
        {
            var param = new UpdateAppRolePermissionsCommand { ApplicationId = form.ApplicationId, RoleId = form.RoleId, PermissionIds = permissionIds };

            await _mediator.Send(param);

            return Ok();
        }
    }
}
