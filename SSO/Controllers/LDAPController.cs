using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Filters;
using SSO.Infrastructure.LDAP;
using SSO.Infrastructure.Settings.Enums;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/[controller]")]
    [ApiController]
    [RealmValidator(Realm = Realm.LDAP)]
    [Authorize(Policy = "RootPolicy")]
    public class LDAPController : ControllerBase
    {
        readonly SynchronizeUsersService _synchronizeUsersService;

        public LDAPController(SynchronizeUsersService synchronizeUsersService)
        {
            _synchronizeUsersService = synchronizeUsersService;
        }

        /// <summary>
        /// Synchronizes users from LDAP users
        /// </summary>
        /// <returns></returns>
        [HttpPost("sync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public IActionResult SyncUsers()
        {
            var api = JobStorage.Current.GetMonitoringApi();

            var processingJobs = api.ProcessingJobs(0, int.MaxValue);

            if (processingJobs.Any(x => x.Value.Job.ToString() == $"{nameof(SynchronizeUsersService)}.{nameof(_synchronizeUsersService.Begin)}"))
                return Accepted(new { status = "Processing", message = $"The request is being processed. Check the status at /hangfire/jobs/enqueued" });

            BackgroundJob.Enqueue(() => _synchronizeUsersService.Begin());

            return Ok();
        }
    }
}
