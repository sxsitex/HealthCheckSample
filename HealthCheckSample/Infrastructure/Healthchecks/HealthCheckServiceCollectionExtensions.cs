using HealthCheck.Extensions;
using HealthCheck.Models;

using HealthCheckSample.Infrastructure.Healthchecks;

using Ninject;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCheckSample.Infrastructure.Healthchecks
{
    public static class BdirectHealthCheckServiceCollectionExtensions
    {
        public static IKernel AddAppHealthChecks(this IKernel kernel)
        {
            kernel.AddHealthCheck()
                .AddCheck<IdentityHealthCheck>(
                    "identity_health_check",
                    failureStatus: HealthStatus.Unhealthy)
                .AddCheck<SignalrHealthCheck>(
                    "signalr_health_check",
                    failureStatus: HealthStatus.Degraded)
                .AddCheck<DatabaseHealthCheck>(
                    "database_health_check",
                    failureStatus: HealthStatus.Unhealthy);


            return kernel;
        }
    }
}