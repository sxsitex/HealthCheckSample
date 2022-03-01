using HealthCheck.Abstarctions;

using Ninject;

using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCheck.Models
{
    public class HealthCheckRegistration
    {
        private Func<IHealthCheck> _factory;
        private TimeSpan _timeout;

        public HealthCheckRegistration(string name, Func<IHealthCheck> factory, HealthStatus? status, IEnumerable<string> tags, TimeSpan? timeout)
        {
            Name = name;
            Factory = factory;
            FailureStatus = status ?? HealthStatus.Unhealthy;
            Timeout = timeout ?? System.Threading.Timeout.InfiniteTimeSpan;
            Tags = new HashSet<string>(tags ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);
        }

        public Func<IHealthCheck> Factory
        {
            get => _factory;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _factory = value;
            }
        }

        public TimeSpan Timeout
        {
            get => _timeout;
            set
            {
                if (value <= TimeSpan.Zero && value != System.Threading.Timeout.InfiniteTimeSpan)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _timeout = value;
            }
        }

        public HealthStatus FailureStatus { get; set; }
        public string Name { get; set; }
        public IHealthCheck Instance { get; set; }
        public ISet<string> Tags { get; }
    }
}
