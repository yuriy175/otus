using Microsoft.AspNetCore.Hosting;
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
                    "RABBITMQ_CONNECTION: " + Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION") +
                    "POSTGRESQL_CONNECTION: " + Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION")
                    );
                webBuilder.UseStartup<Startup>();
            });
    }
}
