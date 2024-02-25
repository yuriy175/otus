using System.Diagnostics;

namespace Bff.API.Middlewares
{
    public class TraceMiddleware
    {
        private readonly RequestDelegate _next;

        public TraceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers["trace-id"] = Activity.Current?.TraceId.ToString();
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }


    public static class TraceMiddlewareExtensions
    {
        public static IApplicationBuilder UseTraceMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<TraceMiddleware>();
        }
    }
}
