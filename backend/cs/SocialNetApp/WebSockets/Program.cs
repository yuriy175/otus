using Microsoft.AspNetCore.Hosting;
using System.Net;
using WebSockets;

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
                    "POSTGRESQL_CONNECTION: " + Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION")+
                    Environment.NewLine +
                    "RABBITMQ_CONNECTION: " + Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION") +
                    Environment.NewLine +
                    "REST_PORT: " + Environment.GetEnvironmentVariable("REST_PORT") +
                    Environment.NewLine +
                    "REDIS_HOST: " + Environment.GetEnvironmentVariable("REDIS_HOST") +
                    Environment.NewLine +
                    "HOSTNAME: " + Dns.GetHostName()
                    );
                webBuilder.UseStartup<Startup>();
            });
    }
}

