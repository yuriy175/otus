using Microsoft.AspNetCore.Hosting;
using Dialogs;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

public static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                Console.WriteLine(
                    "POSTGRESQL_DIALOGDB_CONNECTION: " + Environment.GetEnvironmentVariable("POSTGRESQL_DIALOGDB_CONNECTION") +
                    Environment.NewLine +
                    "REDIS_HOST: " + Environment.GetEnvironmentVariable("REDIS_HOST")+
                    Environment.NewLine +
                    "REST_PORT: " + Environment.GetEnvironmentVariable("REST_PORT") +
                    Environment.NewLine +
                    "REDIS_HOST: " + Environment.GetEnvironmentVariable("REDIS_HOST") +
                    Environment.NewLine +
                    "GRPC_PORT: " + Environment.GetEnvironmentVariable("GRPC_PORT"));
                webBuilder.ConfigureKestrel(options =>
                {
                    options.Listen(IPAddress.Any, Convert.ToInt32(Environment.GetEnvironmentVariable("REST_PORT")), listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                    });
                    options.Listen(IPAddress.Any, Convert.ToInt32(Environment.GetEnvironmentVariable("GRPC_PORT")), listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http2;
                    });
                });
                webBuilder.UseStartup<Startup>();
            });
    }
}