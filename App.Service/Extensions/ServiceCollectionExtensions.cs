using App.Data.Abstract;
using App.Data.Concrete;
using App.Data.Concrete.EntityFramework.Context;
using App.Entities.Concrete;
using App.Service.Abstract;
using App.Service.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace App.Service.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection LoadMyServices(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>();
            services.AddIdentity<User, Role>(options =>
            {
                //User Password Options
                options.Password.RequireDigit = false;//Rakam olsun mu?
                options.Password.RequiredLength = 8; //Kaç karakter min.
                options.Password.RequiredUniqueChars = 0;//Kaç tane özel karakter olsun
                options.Password.RequireNonAlphanumeric = false; //özel karakter zorunlu olsun mu?
                options.Password.RequireLowercase = false; // Küçük harf zorunlu olsun mu?
                options.Password.RequireUppercase = false; // Büyük harf zorunlu olsun mu?
                // User Username ve Email Options
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ "; // User name hangi karakterlerden oluşabilir?
                options.User.RequireUniqueEmail = true; // email unique olsun
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                options.Lockout.MaxFailedAccessAttempts = 3;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(20); //Önemli değişikliklerden sonra kullanıcı 20 dk sonra çıkarılıp tekrar giriş yapması sağlansın.
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<INewsService, NewsManager>();
            services.AddScoped<IPageService, PageManager>();
            services.AddScoped<IContactService, ContactManager>();
            services.AddScoped<INewsCommentService, NewsCommentManager>();
            services.AddScoped<ISendEmailService, SendEmailService>();
            return services;
        }
    }
}
