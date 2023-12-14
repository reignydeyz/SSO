using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Business.Applications.Commands;

namespace SSO.Web.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Authorize(Policy = "RootPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        /// <summary>
        /// Creates new app
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        public IActionResult Create([FromBody] CreateAppCommand param)
        {
            throw new NotImplementedException();
        }
    }
}
