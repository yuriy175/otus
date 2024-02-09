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
        public MetricMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var key = context.Request.Path;
            _counters.AddOrUpdate(key,
                k =>
                {
                    var counter = _meter.CreateCounter<int>(name: $"bff_{k}");
                    counter.Add(1);
                    return counter;
                },
                (k, c) =>
                {
                    c.Add(1);
                    return c;
                });
            await _next(context);
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
