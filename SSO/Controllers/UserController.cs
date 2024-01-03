using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using SSO.Business.Users;
using SSO.Business.Users.Queries;

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
        [Authorize(Policy = "RootPolicy")]
        [EnableQuery]
        public IQueryable<UserDto> Get()
        {
            return _mediator.Send(new GetUsersQuery { }).Result;
        }
    }
}
