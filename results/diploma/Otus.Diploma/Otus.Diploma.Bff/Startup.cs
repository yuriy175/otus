using Bff.API.Middlewares;
using Bff.Infrastructure.gRpc.Services;
using Bff.Infrastructure.gRpc.Services.Interfaces;
using Bff.Infrastructure.Services.Interfaces;
using Common.MQ.Core.Model.Interfaces;
using Common.MQ.Core.Services;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace Bff
{
    public class Startup
    {
        private const string CorsPolicy = "CorsPolicy";

        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy,
                    builder => builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin()
                );
            });

            services.ConfigureSwaggerGen();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton<IMQueue, MQSender>();
            services.AddSingleton<IMeasureService, MeasureService>();
            services.AddSingleton<IGrpcChannelsProvider, GrpcChannelsProvider>();

            //services.AddAuthorization();
            //services.ConfigureAuthentication();

            services.AddOpenTelemetry()
                .WithTracing(tracerProviderBuilder =>
                    tracerProviderBuilder
                        .AddSource(DiagnosticsConfig.ActivitySource.Name)
                                    .ConfigureResource(resource => resource
                                        .AddService(DiagnosticsConfig.ServiceName))
                                                    .AddHttpClientInstrumentation()
                                                    .AddAspNetCoreInstrumentation()
                                                    .AddGrpcClientInstrumentation()
                                                    .AddJaegerExporter());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseCors(CorsPolicy);
            app.UseRouting();
            UseSwaggerDocumentation(app, "/swagger/v1/swagger.json", "Social Net v1");

            //app.UseAuthentication();
            //app.UseAuthorization();
            //app.UseWebSockets();

            app.UseTraceMiddleware();
            app.UseMetricMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        public static IApplicationBuilder UseSwaggerDocumentation(IApplicationBuilder app, string url, string name)
        {
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(url, name);
                });
            return app;
        }
    }
}

public static class DiagnosticsConfig
{
    public const string ServiceName = "BffService";
    public static ActivitySource ActivitySource = new ActivitySource(ServiceName);
}