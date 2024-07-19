using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Filters;
using SSO.Infrastructure.LDAP;
using SSO.Infrastructure.Settings.Enums;
using System.Security.Claims;

namespace SSO.Controllers
{
    [ApiExplorerSettings(GroupName = "System")]
    [Route("api/[controller]")]
    [ApiController]
    [IdpValidator(IdentityProvider = IdentityProvider.LDAP)]
    [Authorize(Policy = "RootPolicy")]
    public class LDAPController : ControllerBase
    {
        readonly SynchronizeService _synchronizeService;

        public LDAPController(SynchronizeService synchronizeService)
        {
            _synchronizeService = synchronizeService;
        }

        /// <summary>
        /// Checks sync if is has completed
        /// </summary>
        /// <returns></returns>
        [HttpGet("sync/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public IActionResult GetSyncStatus()
        {
            /*var api = JobStorage.Current.GetMonitoringApi();

            var processingJobs = api.ProcessingJobs(0, int.MaxValue);

            if (processingJobs.Any(x => x.Value.Job.ToString() == $"{nameof(SynchronizeService)}.{nameof(_synchronizeService.Begin)}"))
                return Accepted(new { status = "Processing", message = $"The request is being processed. Check the status at /hangfire/jobs/enqueued" });*/

            return Ok(new { status = "Ready", message = "Process is ready for further action." });
        }

        /// <summary>
        /// Synchronizes users from LDAP users
        /// </summary>
        /// <returns></returns>
        [HttpPost("sync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public IActionResult Sync()
        {
            var status = GetSyncStatus();

            if (status is AcceptedResult)
                return status;

            var realmId = new Guid(User.Claims.First(x => x.Type == ClaimTypes.System).Value);
            var jobId = BackgroundJob.Enqueue(() => _synchronizeService.Begin(realmId));
            using (var connection = JobStorage.Current.GetConnection())
            {
                connection.SetJobParameter(jobId, "Tags", realmId.ToString());
            }

            return Ok();
        }
    }
}
