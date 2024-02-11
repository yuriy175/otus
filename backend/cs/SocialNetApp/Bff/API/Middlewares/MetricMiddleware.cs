using OpenTelemetry.Metrics;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Bff.API.Middlewares
{
    public class MetricMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Meter _meter = new("Bff_C#", "1.0.0");
        private readonly ConcurrentDictionary<string, Counter<int>> _counters = new ConcurrentDictionary<string, Counter<int>>();
        private readonly ConcurrentDictionary<string, Histogram<long>> _histograms = new ConcurrentDictionary<string, Histogram<long>>();
        public MetricMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var key = context.Request.Path;
            var timer = new Stopwatch();
            timer.Start();
            try
            {
                await _next(context);
            }
            catch
            {
                AddCounter(key, true);
                throw;
            }

            var failed = context.Response.StatusCode >= 500;
            AddCounter(key, failed);
            if (!failed)
            {
                AddHistogram(key, timer.ElapsedMilliseconds);
            }
        }

        private void AddCounter(string key, bool failed = false)
        {
            _counters.AddOrUpdate(key,
                k =>
                {
                    var counter = _meter.CreateCounter<int>(name: $"bff_{k}_{(failed ? "failed" : "success")}");
                    counter.Add(1);
                    return counter;
                },
                (k, c) =>
                {
                    c.Add(1);
                    return c;
                });
        }

        private void AddHistogram(string key, long ms)
        {
            _histograms.AddOrUpdate(key,
                k =>
                {
                    var histogram = _meter.CreateHistogram<long>(name: $"bff_hist_{k}");
                    histogram.Record(ms);
                    return histogram;
                },
                (k, c) =>
                {
                    c.Record(1);
                    return c;
                });
        }
    }


    public static class MetricMiddlewareExtensions
    {
        public static IApplicationBuilder UseMetricMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MetricMiddleware>();
        }
    }
}
