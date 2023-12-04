using Common.MQ.Core.Model.Interfaces;
using Common.MQ.Core.Services;
using Posts.Core.Model.Interfaces;
using Posts.Core.Services;
using Posts.Infrastructure.Caches;
using Posts.Infrastructure.Repositories;
using Posts.Infrastructure.Repositories.Interfaces;
using Profile.Core.Services;
using System.Reflection;
using grpc = Posts.Infrastructure.gRpc.Services;

namespace Posts
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
                Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION") ?? string.Empty,
                Environment.GetEnvironmentVariable("POSTGRESQL_READ_CONNECTION") ?? string.Empty));
            services.AddSingleton<ICacheService, RedisService>();
            services.AddScoped<IFriendsRepository, FriendsRepository>();
            services.AddScoped<IPostsRepository, PostsRepository>();

            services.AddScoped<IFriendsService, FriendsService>();
            services.AddScoped<IPostsService, PostsService>();
            services.AddScoped<IMQSender, MQSender>();            

            services.ConfigureSwaggerGen();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddAuthorization();
            services.ConfigureAuthentication();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<grpc.FriendService>();
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