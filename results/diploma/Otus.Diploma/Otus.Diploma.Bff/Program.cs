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
                    "GRPC_MEASURES: " + Environment.GetEnvironmentVariable("GRPC_MEASURES") +
                    Environment.NewLine +
                    "GRPC_MEASURES2: " + Environment.GetEnvironmentVariable("GRPC_MEASURES2") +
                    Environment.NewLine +
                    "RABBITMQ_CONNECTION: " + Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION") +
                    Environment.NewLine +
                    "OTEL_EXPORTER_JAEGER_ENDPOINT: " + Environment.GetEnvironmentVariable("OTEL_EXPORTER_JAEGER_ENDPOINT") +
                    Environment.NewLine +
                    "OTEL_EXPORTER_JAEGER_AGENT_HOST: " + Environment.GetEnvironmentVariable("OTEL_EXPORTER_JAEGER_AGENT_HOST") +
                    Environment.NewLine);
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