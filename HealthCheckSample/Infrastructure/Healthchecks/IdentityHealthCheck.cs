
using HealthCheck.Abstarctions;
using HealthCheck.Models;

using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheckSample.Infrastructure.Healthchecks
{
    public class IdentityHealthCheck : IHealthCheck
    {
        HttpClient _httpClient;

        public IdentityHealthCheck(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var uri = new Uri("https://google.com");

                var url = $"{uri.GetLeftPart(UriPartial.Authority)}/health";

                var result = await _httpClient.GetAsync(url);

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    return new HealthCheckResult(status: context.Registration.FailureStatus);
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