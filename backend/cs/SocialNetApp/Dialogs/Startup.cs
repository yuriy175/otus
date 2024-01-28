using Common.MQ.Core.Model.Interfaces;
using Common.MQ.Core.Services;
using Dialogs.Core.Model.Interfaces;
using Dialogs.Core.Services;
using Dialogs.Infrastructure.Caches;
using Dialogs.Infrastructure.Repositories;
using Dialogs.Infrastructure.Repositories.Interfaces;
using grpc = Dialogs.Infrastructure.gRpc.Services;

namespace Dialogs
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
            _ = services.AddSingleton<DataContext>(p => new DataContext(
                Environment.GetEnvironmentVariable("POSTGRESQL_DIALOGDB_CONNECTION") ?? string.Empty,
                Environment.GetEnvironmentVariable("POSTGRESQL_DIALOGDB_READ_CONNECTION") ?? string.Empty));
            services.AddSingleton<IDialogsRepository, DialogsRepository>();
            services.AddSingleton<IDialogsService, DialogsService>();
            services.AddSingleton<IMQSender, MQSender>();
            services.AddSingleton<IMQReceiver, MQReceiver>();
            services.AddSingleton<ICacheService, RedisService>();

            services.ConfigureSwaggerGen();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddAuthorization();
            services.ConfigureAuthentication();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseCors(CorsPolicy);
            app.UseRouting();
            UseSwaggerDocumentation(app, "/swagger/v1/swagger.json", "Social Net v1");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<grpc.DialogService>();
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