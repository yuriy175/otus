using Microsoft.AspNetCore.Hosting;
using Bff;
using System.Diagnostics.Metrics;
using OpenTelemetry.Metrics;
using OpenTelemetry;

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
                webBuilder.UseStartup<Startup>();
            });
    }
}