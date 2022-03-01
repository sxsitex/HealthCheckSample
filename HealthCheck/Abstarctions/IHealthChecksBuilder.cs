using HealthCheck.Models;

using Ninject;

using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCheck.Abstarctions
{
    public interface IHealthChecksBuilder
    {
        IHealthChecksBuilder Add(HealthCheckRegistration registration);
        IKernel Kernel { get; }
    }
}
