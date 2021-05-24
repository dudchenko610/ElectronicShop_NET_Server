using ElectronicShopBusinessLogicLayer.Providers;
using ElectronicShopBusinessLogicLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopBusinessLogicLayer
{
    public class Startup
    {
        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            ElectronicShopDataAccessLayer.Startup.Initialize(services, configuration);

            services.AddSingleton<IConfiguration>(provider => configuration);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<AuthService>();
            services.AddTransient<UserService>();
            services.AddTransient<ChatService>();
            services.AddTransient<CategoryService>();
            services.AddTransient<ManufacturerService>();
            services.AddTransient<CharacteristicService>();
            services.AddTransient<CharcterisiticValueService>();
            services.AddTransient<ProductsService>();
            services.AddTransient<ProductCommentService>();
            services.AddTransient<ProductLikeService>();


            services.AddTransient<JwtProvider>();

        }
    }
}
