using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using SSO.Business.ApplicationGroupRoles;
using SSO.Business.ApplicationGroups.Commands;
using SSO.Business.ApplicationGroups.Queries;
using SSO.Business.Groups;
using SSO.Filters;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/application/{applicationId}/group")]
    [ApiController]
    [Authorize(Policy = "RealmAccessPolicy")]
    public class ApplicationGroupController : ControllerBase
    {
        readonly IMediator _mediator;

        public ApplicationGroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets app`s groups 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery(MaxTop = 1000)]
        [ProducesResponseType(typeof(List<GroupDto>), 200)]
        public async Task<IActionResult> Get([FromRoute] Guid applicationId, [FromQuery] Guid? appId = null)
        {
            if (Request.Path.HasValue && Request.Path.Value.Contains("/odata"))
            {
                if (appId is null)
                    throw new ArgumentNullException();

                var res = _mediator.Send(new GetGroupsByApplicationIdQuery { ApplicationId = appId!.Value }).Result;

                return Ok(res);
            }

            var res1 = _mediator.Send(new GetGroupsByApplicationIdQuery { ApplicationId = applicationId }).Result.Take(1000);

            return Ok(res1);
        }

        /// <summary>
        /// Removes group from app
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpDelete("{groupId}")]
        [GroupIdValidator<AppGroupIdDto>]
        public async Task<IActionResult> Delete([FromRoute] AppGroupIdDto form)
        {
            var param = new RemoveAppGroupCommand { GroupId = form.GroupId, ApplicationId = form.ApplicationId };

            await _mediator.Send(param);

            return Ok();
        }
    }
}
