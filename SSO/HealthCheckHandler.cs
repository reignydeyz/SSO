﻿using Microsoft.Extensions.Diagnostics.HealthChecks;
using SSO.Domain.Interfaces;
using System.Globalization;

namespace SSO
{
    public class HealthCheckHandler : IHealthCheck
    {
        readonly IAppDbContext _context;

        public HealthCheckHandler(IAppDbContext context)
        {
            _context = context;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var data = new Dictionary<string, object>
            {
                { "version", $"{typeof(Program).Assembly.GetName().Version}" },
                { "ASPNETCORE_ENVIRONMENT", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") },
                { "dateTime", new { DateTime.UtcNow, DateTime.Now } },
                { "cultureInfo", CultureInfo.CurrentCulture.Name },
                { "appDbContext", _context.Database.CanConnect() },
            };

            return Task.FromResult(HealthCheckResult.Healthy(string.Empty, data));
        }
    }
}
