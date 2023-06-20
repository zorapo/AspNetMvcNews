using App.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Concrete.EntityFramework.DataSeed
{
    public static class DbSeeder
    {

        public static void Seed(ModelBuilder builder)
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                City = "Istanbul",
                Picture = "default.jpg",
                EmailConfirmed = true,               
                SecurityStamp = Guid.NewGuid().ToString()
            };
            user.PasswordHash = CreatePasswordHash(user, "adminadmin");

            var user2 = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "user",
                NormalizedUserName = "USER",
                Email = "user@gmail.com",
                NormalizedEmail = "USER@GMAIL.COM",
                City = "Istanbul",
                Picture = "default.jpg",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()

            };
            user2.PasswordHash = CreatePasswordHash(user2, "useruser");

            var role = new Role
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()

            };

            builder.Entity<Category>().HasData(
                  new Category() { Id = 1, Name = "Politika" },
                  new Category() { Id = 2, Name = "Teknoloji", },
                  new Category() { Id = 3, Name = "Spor", },
                  new Category() { Id = 4, Name = "Sağlık", },
                  new Category() { Id = 5, Name = "Seyahat", }
              );
            //builder.Entity<NewsImage>().HasData(new NewsImage() { Id=1, NewsId=1, CreatedByName="x", IsActive=true, IsDeleted=false, ModifiedByName="x", ImagePath = "/img/news1.jpg" });
            builder.Entity<User>().HasData(user, user2);

            builder.Entity<Role>().HasData(role);

        }
        private static string CreatePasswordHash(User user, string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            return passwordHasher.HashPassword(user, password);
        }
    }
    }
