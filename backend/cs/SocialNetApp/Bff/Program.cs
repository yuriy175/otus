using Microsoft.AspNetCore.Hosting;
using Bff;

public static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
        return Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                Console.WriteLine(
                    "GRPC_PROFILE: " + Environment.GetEnvironmentVariable("GRPC_PROFILE") +
                    Environment.NewLine);
                webBuilder.UseStartup<Startup>();
            });
    }
}