using HealthCheck.Abstarctions;
using HealthCheck.Models;

using Ninject;

using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCheck.Extensions
{
    public static class HealthChecksBuilderAddCheckExtensions
    {
        public static IHealthChecksBuilder AddCheck<T>(
           this IHealthChecksBuilder builder,
           string name,
           HealthStatus? failureStatus = null,
           IEnumerable<string> tags = null,
           TimeSpan? timeout = null) where T : class, IHealthCheck
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            builder.Kernel.Bind<T>().ToSelf().InSingletonScope();

            return builder.Add(new HealthCheckRegistration(name, () => builder.Kernel.Get<T>() , failureStatus, tags, timeout));
        }
    }
}
