using System;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace ECommerce.Api.Search.HealthCheckProvider
{
    public static class DbHealthCheckProvider
    {
        public static HealthCheckResult Check(string connectionString)
        {
            return HealthCheckResult.Healthy();
        }
    }
}
