using Common.MQ.Core.Model.Interfaces;
using Common.MQ.Core.Services;
using Measure.Core.Model.Interfaces;
using Measure.Core.Services;
using Measure.Infrastructure.Repositories;
using Measure.Infrastructure.Repositories.Interfaces;
using grpc = Measure.Infrastructure.gRpc.Services;

namespace Measure
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
            services.AddGrpc();
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

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            _ = services.AddSingleton<DataContext>(p => new DataContext(
                Environment.GetEnvironmentVariable("POSTGRESQL_READ_CONNECTION") ?? string.Empty,
                Environment.GetEnvironmentVariable("POSTGRESQL_READ_CONNECTION2") ?? string.Empty));

            services.AddSingleton<IMQueue, MQReceiver>();
            services.AddSingleton<IMeasureRepository, MeasureRepository>();
            services.AddSingleton<IMeasureService, MeasureService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseCors(CorsPolicy);
            app.UseRouting();

            //var t = new MeasureRepository(new DataContext(Environment.GetEnvironmentVariable("POSTGRESQL_READ_CONNECTION")));
            //var tt = t.GetMeasuresAsync(999, CancellationToken.None).Result;
            app.ApplicationServices.GetService<IMeasureService>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<grpc.MeasureService>();
            });
        }
    }
}