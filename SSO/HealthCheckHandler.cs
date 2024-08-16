using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SSO.Business.Versions.Queries;
using SSO.Domain.Interfaces;
using System.Globalization;

namespace SSO
{
    public class HealthCheckHandler : IHealthCheck
    {
        readonly IAppDbContext _context;
        readonly IMediator _mediator;

        public HealthCheckHandler(IAppDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var data = new Dictionary<string, object>
            {
                { "version", $"{typeof(Program).Assembly.GetName().Version}" },
                { "latestVersion", _mediator.Send(new GetLatestVersionQuery()).Result },
                { "ASPNETCORE_ENVIRONMENT", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") },
                { "dateTime", new { DateTime.UtcNow, DateTime.Now } },
                { "cultureInfo", CultureInfo.CurrentCulture.Name },
                { "appDbContext", _context.Database.CanConnect() },
                { "platform", Environment.OSVersion.Platform },
            };

            return Task.FromResult(HealthCheckResult.Healthy(string.Empty, data));
        }
    }
}
