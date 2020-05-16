using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ECommerce.Api.Search.HealthCheckProvider
{
    public abstract class DbConnectionHealthCheck : IHealthCheck
    {

        protected DbConnectionHealthCheck(string connectionString) : this(connectionString, testQuery: null)
        { 
        }

        protected DbConnectionHealthCheck(string connectionString, string testQuery)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            TestQuery = testQuery;
        }

        protected string ConnectionString { get;  }
        protected string TestQuery { get;  }
        protected abstract DbConnection CreateConnection(string connectionString);
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using(var connection = CreateConnection(ConnectionString))
            {
                try
                {
                    await connection.OpenAsync(cancellationToken);
                    if (TestQuery != null)
                    {
                        var command = connection.CreateCommand();
                        command.CommandText = TestQuery;
                        await command.ExecuteNonQueryAsync(cancellationToken);
                    }
                }
                catch (DbException ex)
                {
                    return new HealthCheckResult(status: context.Registration.FailureStatus, exception: ex);
                }

            }

            return HealthCheckResult.Healthy();
        }
    }
}