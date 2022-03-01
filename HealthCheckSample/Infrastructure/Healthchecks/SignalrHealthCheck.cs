
using HealthCheck.Abstarctions;
using HealthCheck.Models;

using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheckSample.Infrastructure.Healthchecks
{
    public class SignalrHealthCheck : IHealthCheck
    {
        HttpClient _httpClient;

        public SignalrHealthCheck(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                
                var urls = "https://google.com".Split(',');

                if (urls == null || !urls.Any())
                {
                    throw new Exception();
                }

                foreach (var url in urls)
                {
                    var response = await _httpClient.GetAsync($"{url}");

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception();
                    }
                }
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(status: context.Registration.FailureStatus, exception: ex);
            }

            return HealthCheckResult.Healthy();
        }
    }
}