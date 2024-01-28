using Common.MQ.Core.Model.Interfaces;
using Common.MQ.Core.Services;
using System.Net.WebSockets;
using System.Text;
using Websockets.Core.Model.Interfaces;
using WebSockets;
using WebSockets.Core.Services;
using WebSockets.Infrastructure.Caches;
using WebSockets.Infrastructure.Repositories;
using WebSockets.Infrastructure.Repositories.Interfaces;

namespace WebSockets
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

            services.AddAuthorization();
            services.ConfigureAuthentication();

            _ = services.AddSingleton<DataContext>(p => new DataContext(
                Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION") ?? string.Empty,
                Environment.GetEnvironmentVariable("POSTGRESQL_READ_CONNECTION") ?? string.Empty));

            services.AddSingleton<IMQReceiver, MQReceiver>();
            services.AddSingleton<IMQSender, MQSender>();
            services.AddSingleton<FeedPostsWebsocketsService>();
            services.AddSingleton<DialogsWebsocketsService>(); 
            services.AddSingleton<IFriendsRepository, FriendsRepository>();
            services.AddSingleton<ICacheService, RedisService>();
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

            app.UseWebSockets();

            app.Run(async (context) => {
                if (context.Request.Path == "/dialogs")
                {
                    var webService = app.ApplicationServices.GetRequiredService<DialogsWebsocketsService>();
                    var t = await webService.OnWebSocketConnectAsync(context);
                    await t;
                } else if (context.Request.Path == "/post/feed")
                {
                    var webService = app.ApplicationServices.GetRequiredService<FeedPostsWebsocketsService>();
                    var t = await webService.OnWebSocketConnectAsync(context);
                    await t;
                }
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