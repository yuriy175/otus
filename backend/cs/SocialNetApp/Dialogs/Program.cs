using Microsoft.AspNetCore.Hosting;
using Dialogs;

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
                    "REDIS_HOST: " + Environment.GetEnvironmentVariable("REDIS_HOST"));
                webBuilder.UseStartup<Startup>();
            });
    }
}