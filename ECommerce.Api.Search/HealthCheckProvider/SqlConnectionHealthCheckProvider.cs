using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace ECommerce.Api.Search.HealthCheckProvider
{
    public class SqlConnectionHealthCheckProvider : DbConnectionHealthCheck
    {
        private static readonly string DefaultTestQuery = "Select 1";

        public SqlConnectionHealthCheckProvider(string connectionString) : this(connectionString, testQuery: DefaultTestQuery)
        {
        }

        public SqlConnectionHealthCheckProvider(string connectionString, string testQuery)
            : base(connectionString, testQuery ?? DefaultTestQuery)
        {
        }

        protected override DbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
