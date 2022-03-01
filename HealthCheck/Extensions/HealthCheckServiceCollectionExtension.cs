using HealthCheck.Abstarctions;
using HealthCheck.Models;

using Ninject;

using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCheck.Extensions
{
    public static class HealthCheckServiceCollectionExtension
    {
        public static IHealthChecksBuilder AddHealthCheck(this IKernel kernel)
        {
            kernel.Bind<HealthCheckServiceOptions>().ToSelf().InSingletonScope();
            kernel.Bind<IHealthCheckService>().To<HealthCheckService>().InSingletonScope();

            return new HealthChecksBuilder(kernel);
        }
    }
}
