
using HealthCheck.Abstarctions;
using HealthCheck.Models;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheckSample.Infrastructure.Healthchecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly string _helthCheckSql = @"select * from master";

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            IReadOnlyDictionary<string, object> healthchecDescription;

            try
            {
               using (var connection = new SqlConnection("ConnectionString"))
                {
                    connection.Open();

                    var command = new SqlCommand(_helthCheckSql, connection);

                    var result = await command.ExecuteScalarAsync();

                    healthchecDescription = new Dictionary<string, object> {
                         { "site", (string)result },
                    };

                }
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(status: context.Registration.FailureStatus, exception: ex);
            }

            return HealthCheckResult.Healthy(data:healthchecDescription);
        }
    }
}