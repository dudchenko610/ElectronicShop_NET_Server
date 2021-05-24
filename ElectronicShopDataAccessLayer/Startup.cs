using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ElectronicShopDataAccessLayer.Core;
using ElectronicShopDataAccessLayer.Services;
using ElectronicShopDataAccessLayer.Repositories;
using ElectronicShopDataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Shared.Constants;
using ElectronicShopDataAccessLayer.Contexts;

namespace ElectronicShopDataAccessLayer
{
    public class Startup
    {
        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            string connection = configuration.GetConnectionString("SQLServerConnection");
            services.AddDbContext<DbContextSql>(builder =>
                builder.UseSqlServer(connection)
            );

            services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = Constants.Password.MIN_LENGTH_PASSWORD;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;

                opts.User.RequireUniqueEmail = true;
                opts.User.AllowedUserNameCharacters = Constants.Password.PASSWORD_VALID;

            })
            .AddEntityFrameworkStores<DbContextSql>();
            services.AddTransient<DbContextMongo>();



            services.AddTransient<UserRepository>();
            services.AddTransient<MessageRepository>();
            services.AddTransient<CategoryRepository>();
            services.AddTransient<ManufacturerRepository>();
            services.AddTransient<CharacteristicRepository>();
            services.AddTransient<CharacteristicValueRepository>();
            services.AddTransient<ProductsRepository>();
            services.AddTransient<ProductPhotoRepository>();
            services.AddTransient<ProductCommentRepository>();
            services.AddTransient<ProductLikeRepository>();
            services.AddTransient<N_N_ProductCharacteristicValueRepository>();

            services.AddTransient<FileService>();

            services.AddHttpContextAccessor();

        }
    }
}
