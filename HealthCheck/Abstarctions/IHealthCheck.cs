using HealthCheck.Models;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck.Abstarctions
{
    public interface IHealthCheck
    {
        Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default);
    }
}
