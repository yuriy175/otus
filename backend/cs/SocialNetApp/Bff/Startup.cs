using Bff.API.Middlewares;
using Bff.Infrastructure.gRpc.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Profile.Infrastructure.gRpc.Services;
using System.Reflection;

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

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFriendService, FriendService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IDialogService, DialogService>();            

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
            app.UseWebSockets();
            Parallel.Invoke(
                () => FriendService.WarmupChannels(), 
                () => UserService.WarmupChannels(),
                () => PostService.WarmupChannels(),
                () => DialogService.WarmupChannels()
                );

            app.UseHeadersMiddleware();

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