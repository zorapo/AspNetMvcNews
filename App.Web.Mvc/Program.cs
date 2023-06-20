using App.Data.Concrete.EntityFramework.Context;
using App.Entities.OptionsModels;
using App.Service.AutoMapper.Profiles;
using App.Service.Extensions;
using App.Web.Mvc.AutoMapper.Profiles;
using App.Web.Mvc.Helpers.Abstract;
using App.Web.Mvc.Helpers.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// AddAsync services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddAutoMapper(typeof(CategoryProfile), typeof(NewsProfile),typeof(UserProfile));
builder.Services.LoadMyServices();
builder.Services.AddScoped<IImageHelper,ImageHelper>();
builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = new PathString("/Auth/Login"); // Login sayfa url'si
	options.LogoutPath = new PathString("/Auth/Logout"); //Logout sayfa url'si
	options.Cookie = new CookieBuilder
	{
		Name="AspNetMvcNews", //Cookie ismi
		HttpOnly = true, // Client-side tarafýnda cookie bilgilerini gizlemek, korumak için
		SameSite= SameSiteMode.Strict, //Bizim cookie'lerimizi kullanarak farklý yerden gelmesini önleyen güvenlik önlemi
		SecurePolicy=CookieSecurePolicy.SameAsRequest //Gelen istek http üzerinden gelirse http , https üzerinden gelirse https üzerinden dönüþ yapar. Güvenlik açýsýndan ".Always" olmalý.
	
	};
	options.SlidingExpiration = true; //Giriþ yaptýktan sonra tekrar giriþ yapýlýrsa Cookie süresi tekrar 5 gün uzar
	options.ExpireTimeSpan = TimeSpan.FromDays(5); // Tarayýcý üzerinde Cooki bilgileri 5 gün kalacak
	options.AccessDeniedPath = new PathString("/Auth/AccessDenied"); // Giriþ yapan kullanýcý yetkisi olmayan bir yere giriþ yaparsa buraya yönlendirilecek
});
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConStr"));
});
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
{
	opt.TokenLifespan = TimeSpan.FromHours(2); // Þifremi unuttum için Token 2 saat ömrü var
});

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
var app = builder.Build();

//Son migration yapýlmamýþsa otomatik yapar.
using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	db.Database.Migrate();
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
app.UseSession();//Server'da session oluþmasýný istiyoruz
app.UseStatusCodePages(); //Olmayan sayfaya gidilirse 404 hatasý sayfasý açýlýr
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
	name: "admin",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
		  );
//app.MapControllerRoute(
//    name: "settings",
//    pattern: "{PasswordChage}",
//    defaults: new { area="Admin", controller = "Setting", action = "PasswordChange" });
         
app.MapControllerRoute(
	name: "news",
	pattern: "{News}/{Details}/{title}/{id}",
	defaults: new { controller = "News", action = "Detail" });

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");




app.Run();
