using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ECommerce.Api.Search.HealthCheckProvider
{
    public class SendGridHealthCheckProvider : IHealthCheck
    {
        public SendGridHealthCheckProvider()
        {
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}
