using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicShop.Core;
using ElectronicShop.Core.SignalR;
using ElectronicShop.Hubs;
using ElectronicShopPresentationLayer.Middlewares;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Shared.Constants;
using Shared.Options;

namespace ElectronicShop
{
    public class Startup
    {

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins2";
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();

            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

            ElectronicShopBusinessLogicLayer.Startup.Initialize(services, Configuration);


            services.Configure<JwtConnectionOptions>(Configuration.GetSection("jwtConfig"));
            services.Configure<MongoConnectionOptions>(Configuration.GetSection("mongoConfig"));


            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })

            .AddJwtBearer(options =>
            {

                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromMinutes(Constants.Numbers.START_VALUE),
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["jwtConfig:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["jwtConfig:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["jwtConfig:SecretKey"])),
                    ValidateIssuerSigningKey = true
                };
            });


            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                        //.AllowAnyOrigin()
                        .WithOrigins("http://localhost:3000", "http://6f727380c187.ngrok.io")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                     //   .WithOrigins("http://e93cad08086e.ngrok.io");
                    });
            });

            services.AddControllersWithViews();

            

        }

        private async Task AuthQueryStringToHeader(HttpContext context, Func<Task> next)
        {
            var qs = context.Request.QueryString;

            if (string.IsNullOrWhiteSpace(context.Request.Headers["Authorization"]) && qs.HasValue)
            {

                Console.WriteLine("TOKEN = " + qs);

                var token = (from pair in qs.Value.TrimStart('?').Split('&')
                             where pair.StartsWith("token=")
                             select pair.Substring(6)).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(token))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
                }
            }

            await next?.Invoke();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {



            app.UseMiddleware<PingMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStaticFiles();
            app.UseDefaultFiles();

            

            app.UseRouting();


            app.UseCors();

            //   app.UseCors(builder => builder.AllowAnyOrigin());



            // app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseMiddleware<SignalRAuthMiddleware>(); // before authorization

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<LogMiddleware>();

            // app.UseResponseCaching();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHub<ChatHub>("/chatting");
            });


            
        }
    }
}
