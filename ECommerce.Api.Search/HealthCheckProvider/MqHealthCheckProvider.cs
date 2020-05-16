using System;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace ECommerce.Api.Search.HealthCheckProvider
{
    public static class MqHealthCheckProvider
    {
        public static HealthCheckResult Check(string connectionString)
        {
            return HealthCheckResult.Unhealthy();
        }
    }
}
