using Microsoft.EntityFrameworkCore;
using Rajby_web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddControllersWithViews();

// Configure Entity Framework to use SQL Server
builder.Services.AddDbContext<RajbyTextileContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RajbyDev")));

// Add Authentication with Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
      options.LoginPath = "/Auth/LoginBasic";  // Path to redirect if not authenticated
      options.AccessDeniedPath = "/Auth/LoginBasic";  // Path if access is denied
    });

builder.Services.AddAuthorization();  // Add Authorization Services

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use Authentication Middleware
app.UseAuthentication();  // This should come before UseAuthorization()

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=LoginBasic}/{id?}");

app.Run();
