using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.ApplicationRoles;
using SSO.Filters;
using SSO.Business.ApplicationGroupRoles;
using SSO.Business.ApplicationGroupRoles.Queries;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/application/{applicationId}/group/{groupId}/role")]
    [ApiController]
    [AppUserIdValidator]
    [Authorize(Policy = "RootPolicy")]
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
    }
}
