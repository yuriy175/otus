using Microsoft.AspNetCore.Hosting;
using Bff;
using System.Diagnostics.Metrics;
using OpenTelemetry.Metrics;
using OpenTelemetry;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

public static class Program
{
    public static void Main(string[] args)
    {
        using MeterProvider meterProvider = Sdk.CreateMeterProviderBuilder()
                .AddMeter("Bff_C#")
                //.AddPrometheusHttpListener(options => options.UriPrefixes = new string[] { "http://localhost:5298/" })
                .AddPrometheusHttpListener(options => options.UriPrefixes = new string[] { "http://bff_cs:5298/" })
                .Build();

        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                Console.WriteLine(                    
                    "REST_PORT: " + Environment.GetEnvironmentVariable("REST_PORT") +
                    Environment.NewLine +
                    "GRPC_PROFILE: " + Environment.GetEnvironmentVariable("GRPC_PROFILE") +
                    Environment.NewLine +
                    "GRPC_POSTS: " + Environment.GetEnvironmentVariable("GRPC_POSTS") +
                    Environment.NewLine+ 
                    "GRPC_DIALOGS: " + Environment.GetEnvironmentVariable("GRPC_DIALOGS") +
                    Environment.NewLine +
                    "GRPC_COUNTERS: " + Environment.GetEnvironmentVariable("GRPC_COUNTERS") +
                    Environment.NewLine +
                    "OTEL_EXPORTER_JAEGER_ENDPOINT: " + Environment.GetEnvironmentVariable("OTEL_EXPORTER_JAEGER_ENDPOINT") +
                    Environment.NewLine +
                    "OTEL_EXPORTER_JAEGER_AGENT_HOST: " + Environment.GetEnvironmentVariable("OTEL_EXPORTER_JAEGER_AGENT_HOST") +
                    Environment.NewLine);
                //webBuilder.UseStartup<Startup>();
                webBuilder.ConfigureKestrel(options =>
                {
                    options.Listen(IPAddress.Any, Convert.ToInt32(Environment.GetEnvironmentVariable("REST_PORT")), listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http1;
                    });
                });
                webBuilder.UseStartup<Startup>();
            });
    }
}