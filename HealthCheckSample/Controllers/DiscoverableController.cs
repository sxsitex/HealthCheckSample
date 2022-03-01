using HealthCheck.Abstarctions;
using HealthCheck.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HealthCheckSample.Controllers
{
    [RoutePrefix("")]
    public class DiscoverableController : ApiController
    {
        IHealthCheckService _healthCheckService;

        public DiscoverableController(IHealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService ?? throw new ArgumentNullException(nameof(healthCheckService));
        }

        [HttpGet]
        [Route("health")]
        public async Task<HttpResponseMessage> HealthCheckAsync()
        {
            var result = await _healthCheckService.CheckHealthAsync();

            var json = HealthCheckResponseWriter(result);

            var response = Request.CreateResponse(result.Status == HealthStatus.Unhealthy
                ? HttpStatusCode.InternalServerError : HttpStatusCode.OK);
            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return response;
        }

        private static string HealthCheckResponseWriter(HealthReport result)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);

            using (var writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                writer.WritePropertyName("status");
                writer.WriteValue(result.Status.ToString());
                writer.WritePropertyName("results");
                writer.WriteStartObject();
                foreach (var entry in result.Entries)
                {
                    writer.WritePropertyName(entry.Key);
                    writer.WriteStartObject();
                    writer.WritePropertyName("status");
                    writer.WriteValue(entry.Value.Status.ToString());

                    if (entry.Value.Data.Any())
                    {
                        writer.WritePropertyName("data");
                        writer.WriteStartObject();

                        foreach (var d in entry.Value.Data)
                        {
                            writer.WritePropertyName(d.Key);
                            writer.WriteValue(d.Value);
                        }

                        writer.WriteEndObject();
                    }

                    writer.WriteEndObject();
                }
                writer.WriteEndObject();
                writer.WriteEndObject();
            }

            return sb.ToString();
        }
    }
}