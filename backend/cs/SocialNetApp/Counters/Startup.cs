using Common.MQ.Core.Model.Interfaces;
using Common.MQ.Core.Services;
using Counters.Core.Model.Interfaces;
using Counters.Core.Services;
using Counters.Infrastructure.Repositories;
using Counters.Infrastructure.Repositories.Interfaces;
using grpc = Counters.Infrastructure.gRpc.Services;

namespace Counters
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

            services.AddAuthorization();

            _ = services.AddSingleton<DataContext>(p => new DataContext(
                Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION") ?? string.Empty,
                Environment.GetEnvironmentVariable("POSTGRESQL_READ_CONNECTION") ?? string.Empty));

            services.AddSingleton<IMQReceiver, MQReceiver>();
            services.AddScoped<ICountersRepository, CountersRepository>();
            services.AddScoped<ICounterService, CounterService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseCors(CorsPolicy);
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<grpc.CounterService>();
            });
        }
    }
}