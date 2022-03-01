using HealthCheck.Abstarctions;
using HealthCheck.Models;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck
{
    internal class HealthCheckService : IHealthCheckService
    {
        HealthCheckServiceOptions _options;

        public HealthCheckService (HealthCheckServiceOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<HealthReport> CheckHealthAsync(CancellationToken cancellationToken = default)
        {
            var registrations = _options.Registrations;

            var totalTime = ValueStopwatch.StartNew();

            var tasks = new Task<HealthReportEntry>[registrations.Count];
            var index = 0;

            foreach (var registration in registrations)
            {
                tasks[index++] = RunCheckAsync(registration, cancellationToken);
            }

            await Task.WhenAll(tasks);

            index = 0;

            var entries = new Dictionary<string, HealthReportEntry>(StringComparer.OrdinalIgnoreCase);

            foreach (var registration in registrations)
            {
                entries[registration.Name] = tasks[index++].Result;
            }

            var totalElapsedTime = totalTime.GetElapsedTime();

            var report = new HealthReport(entries, totalElapsedTime);

            return report;
        }

        private async Task<HealthReportEntry> RunCheckAsync(HealthCheckRegistration registration, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var healthCheck = registration.Factory();
            
                var stopwatch = ValueStopwatch.StartNew();
                var context = new HealthCheckContext { Registration = registration };

                HealthReportEntry entry;
                CancellationTokenSource timeoutCancellationTokenSource = null;
                try
                {
                    HealthCheckResult result;

                    var checkCancellationToken = cancellationToken;
                    if (registration.Timeout > TimeSpan.Zero)
                    {
                        timeoutCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                        timeoutCancellationTokenSource.CancelAfter(registration.Timeout);
                        checkCancellationToken = timeoutCancellationTokenSource.Token;
                    }

                    result = await healthCheck.CheckHealthAsync(context, checkCancellationToken).ConfigureAwait(false);

                    var duration = stopwatch.GetElapsedTime();

                    entry = new HealthReportEntry(
                        status: result.Status,
                        description: result.Description,
                        duration: duration,
                        exception: result.Exception,
                        data: result.Data,
                        tags: registration.Tags);
                    
                }
                catch (OperationCanceledException ex) when (!cancellationToken.IsCancellationRequested)
                {
                    var duration = stopwatch.GetElapsedTime();
                    entry = new HealthReportEntry(
                        status: HealthStatus.Unhealthy,
                        description: "A timeout occured while running check.",
                        duration: duration,
                        exception: ex,
                        data: null);
                }

                // Allow cancellation to propagate if it's not a timeout.
                catch (Exception ex) when (ex as OperationCanceledException == null)
                {
                    var duration = stopwatch.GetElapsedTime();
                    entry = new HealthReportEntry(
                        status: HealthStatus.Unhealthy,
                        description: ex.Message,
                        duration: duration,
                        exception: ex,
                        data: null);
                }

                finally
                {
                    timeoutCancellationTokenSource?.Dispose();
                }

                return entry;
        }
    }
}
