using App.Data.Concrete.EntityFramework.DataSeed;
using App.Entities.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Concrete.EntityFramework.Context
{
    public class AppDbContext : IdentityDbContext<User, Role, string>
    {
        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            DbSeeder.Seed(builder);
      

		}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-NNOFQ3O\SQL2019;Initial Catalog=AspNetMvcNews;Persist Security Info=True;User ID=sa;Password=123");
        }


        public DbSet<Category> Category { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsComment> NewsComment { get; set; }
        public DbSet<NewsImage> NewsImage { get; set; }
        public DbSet<Page> Page { get; set; }
        public DbSet<Setting> Setting { get; set; }
        public DbSet<Contact> Contact { get; set; }
        

    }
}
