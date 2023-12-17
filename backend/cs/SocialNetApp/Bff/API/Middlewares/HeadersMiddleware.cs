namespace Bff.API.Middlewares
{
    public class HeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public HeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers["Server-Language"] = "CSharp";
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }


    public static class HeadersMiddlewareExtensions
    {
        public static IApplicationBuilder UseHeadersMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HeadersMiddleware>();
        }
    }
}
