using HealthCheck.Abstarctions;
using HealthCheck.Models;

using Ninject;

using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCheck
{
    internal class HealthChecksBuilder : IHealthChecksBuilder
    {
        public HealthChecksBuilder(IKernel kernel)
        {
            Kernel = kernel;
        }

        public IKernel Kernel { get; }

        public IHealthChecksBuilder Add(HealthCheckRegistration registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            var options = Kernel.Get<HealthCheckServiceOptions>();

            options.Registrations.Add(registration);

            return this;
        }

       
    }
}
