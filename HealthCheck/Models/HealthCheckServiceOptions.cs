using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCheck.Models
{
    public sealed class HealthCheckServiceOptions
    {
        public ICollection<HealthCheckRegistration> Registrations { get; } = new List<HealthCheckRegistration>();
    }
}
