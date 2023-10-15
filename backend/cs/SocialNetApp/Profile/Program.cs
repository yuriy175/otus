using Microsoft.AspNetCore.Hosting;
using SocialNetApp;

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
                    "POSTGRESQL_CONNECTION: "+Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION")+
                    Environment.NewLine+
                    "REDIS_HOST: "+Environment.GetEnvironmentVariable("REDIS_HOST"));
                webBuilder.UseStartup<Startup>();
            });
    }
}