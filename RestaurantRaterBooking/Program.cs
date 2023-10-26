using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterBooking.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppContextConnection") ?? throw new InvalidOperationException("Connection string 'AppContextConnection' not found.");

builder.Services.AddDbContext<RestaurantRaterBooking.Models.AppContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<RestaurantRaterBooking.Models.AppContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        var ggconfig = builder.Configuration.GetSection("Authentication:Google");
        options.ClientId = ggconfig["ClientId"];
        options.ClientSecret = ggconfig["ClientSecret"];
        //https://localhost:7116/sign-in-google
        options.CallbackPath = "/sign-in-google";
    })
    .AddFacebook(options =>
    {
        var fconfig = builder.Configuration.GetSection("Authentication:Facebook");
        options.ClientId = fconfig["AppId"];
        options.ClientSecret = fconfig["AppSecret"];
        //https://localhost:7116/sign-in-facebook
        options.CallbackPath = "/sign-in-facebook/";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "area",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
