using CrystalClarityEyewearWebApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestaurantRaterBooking.Models;
using RestaurantRaterBooking.Services;
using System.Configuration;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppContextConnection") ?? throw new InvalidOperationException("Connection string 'AppContextConnection' not found.");

builder.Services.AddDbContext<RestaurantRaterBooking.Models.AppContext>(options => options.UseSqlServer(connectionString));
//builder.Services.AddTransient<RestaurantRaterBooking.Models.AppContext>();

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<RestaurantRaterBooking.Models.AppContext>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<RestaurantRaterBooking.Models.AppContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
	options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
//builder.Services.AddTransient<HtmlEncoder>(provider => HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
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

builder.Services.AddOptions();                                        // Kích hoạt Options
var mailsettings = builder.Configuration.GetSection("MailSettings");  // đọc config
builder.Services.Configure<MailSettings>(mailsettings);               // đăng ký để Inject
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IEmailSender, SendMailService>();

builder.Services.AddRazorPages();
builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseRouting();
app.UseSession();
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
