using ElectronicShopDataAccessLayer.Models;
using ElectronicShopDataAccessLayer.ViewModels.Product.Query;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using System;

using System.Linq;

namespace ElectronicShopDataAccessLayer.Contexts
{
    public class DbContextSql : IdentityDbContext<User>
    {
        public DbContextSql(DbContextOptions<DbContextSql> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbContextSql() : base()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductJoined>().HasNoKey();

            string ADMIN_ROLE_ID = Guid.NewGuid().ToString();
            string CLIENT_ROLE_ID = Guid.NewGuid().ToString();

            string ADMIN_USER_ID = Guid.NewGuid().ToString();

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = ADMIN_ROLE_ID,
                Name = Constants.AuthRoles.ADMIN,
                NormalizedName = Constants.AuthRoles.ADMIN
            });

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = CLIENT_ROLE_ID,
                Name = Constants.AuthRoles.CLIENT,
                NormalizedName = Constants.AuthRoles.CLIENT
            });


            var hasher = new PasswordHasher<User>();



            modelBuilder.Entity<User>().HasData(new User
            {
                Id = ADMIN_USER_ID,
                Name = "Admin",
                Surname = "Admin",
                UserName = "0684261065",
                PhoneNumber = "0684261065",
                NormalizedUserName = "0684261065".ToUpper(),
                Email = "admin@x.com",
                NormalizedEmail = "admin@x.com".ToUpper(),
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "admin"),
                SecurityStamp = string.Empty
            });


            //seed admin into role
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ADMIN_ROLE_ID,
                UserId = ADMIN_USER_ID
            });

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Cascade;
            }

            modelBuilder.Entity<Product>()
               .HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Characteristic>()
               .HasOne(p => p.Category)
               .WithMany(c => c.Characteristics)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CharacteristicValue>()
                .HasOne(cv => cv.Characteristic)
                .WithMany(c => c.CharacteristicValues)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ProductPhoto>()
                .HasOne(pf => pf.Product)
                .WithMany(p => p.ProductPhotos)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductComment>()
                .HasOne(pm => pm.Product)
                .WithMany(p => p.ProductComments)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductLike>()
                .HasOne(pm => pm.Product)
                .WithMany(p => p.ProductLikes)
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<N_N_Product_CharacteristicValue>()
                .HasOne(nn => nn.Product)
                .WithMany(p => p.N_N_Product_Characteristics)
                .OnDelete(DeleteBehavior.Cascade);




            modelBuilder.Entity<N_N_Product_CharacteristicValue>()
                .HasOne(nn => nn.CharacteristicValue)
                .WithMany(p => p.N_N_Product_Characteristics)
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<ProductComment>()
                .HasOne(pc => pc.User)
                .WithMany(us => us.ProductComments)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Chat>()
                .HasOne(c => c.User1)
                .WithMany(us => us.Chats1)
                .HasForeignKey(us => us.UserId1)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.User2)

                .WithMany(us => us.Chats2)
                .HasForeignKey(us => us.UserId2)
                .OnDelete(DeleteBehavior.Restrict);

        }



        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<ProductLike> ProductLikes { get; set; }

        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Characteristic> Characteristics { get; set; }
        public DbSet<N_N_Product_CharacteristicValue> N_N_Product_Characteristics { get; set; }
        public DbSet<CharacteristicValue> CharacteristicValues { get; set; }


        public DbSet<OrderAuth> OrdersAuth { get; set; }
        public DbSet<OrderAuthItem> OrderAuthItems { get; set; }

        public DbSet<OrderNotauth> OrdersNotauth { get; set; }
        public DbSet<OrderNotauthItem> OrderNotauthItems { get; set; }



        public DbSet<Chat> Chats { get; set; }


        public DbSet<ProductJoined> ProductJoineds { get; set; }
    }
}
