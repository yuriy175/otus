using Profile.Infrastructure.Repositories.Interfaces;
using Profile.Infrastructure.Repositories;
using Profile.Core.Model.Interfaces;
using Profile.Core.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.OpenApi.Models;
using Profile.Infrastructure.Caches;

namespace SocialNetApp
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
            _ = services.AddSingleton<DataContext>(p => new DataContext(
                Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION") ?? string.Empty,
                Environment.GetEnvironmentVariable("POSTGRESQL_READ_CONNECTION") ?? string.Empty));
            services.AddSingleton<ICacheService, RedisService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILoadDataRepository, LoadDataRepository>();
            services.AddScoped<IFriendsRepository, FriendsRepository>();
            services.AddScoped<IPostsRepository, PostsRepository>();

            services.AddScoped<ILoadDataService, LoadDataService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFriendsService, FriendsService>();
            services.AddScoped<IPostsService, PostsService>();            

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