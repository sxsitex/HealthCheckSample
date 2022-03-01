using HealthCheck.Models;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck.Abstarctions
{
    public interface IHealthCheckService
    {
        Task<HealthReport> CheckHealthAsync(CancellationToken cancellationToken = default);
    }
}
