using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ShoppingBasket.App.Middlewares;
using ShoppingBasket.CommonHelper;
using ShoppingBasket.DataAccessLayer;
using ShoppingBasket.DataAccessLayer.Infrastructure.IRepository;
using ShoppingBasket.DataAccessLayer.Infrastructure.Repository;
using Stripe;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// For ignoring circular references cycle error
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// custom middleware for adding carts count to the cookie
builder.Services.AddScoped<CookieMiddleware>();

// For resolving IEmailSender exception on activating RegisterModel.
builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();

// configure the stripe payment settings from appsettings.json file.
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("PaymentSettings"));

// For resolving issues of Razor pages services.
builder.Services.AddRazorPages();

// enable memory cache so that this session which is created for email sending, store into browser cache.
builder.Services.AddDistributedMemoryCache();

// creating sessions for email sending 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// must be given so that return url works properly
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
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

// stripe api configuration
StripeConfiguration.ApiKey = builder.Configuration.GetSection("PaymentSettings:SecretKey").Get<string>();

app.UseRouting();
app.UseAuthentication(); ;

app.UseSession();  // enabling session using

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();