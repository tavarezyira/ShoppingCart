using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System;

class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Agregar servicios de MVC
        builder.Services.AddControllersWithViews();

        // Configurar el DbContext con SQLite
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite("Data Source=shoppingcart.db"));


        // Configurar la autenticación: cookies y Google OAuth
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Account/CustomerLogin";
            options.LogoutPath = "/Account/Logout";
            options.AccessDeniedPath = "/Account/CustomerLogin";
        })
        .AddGoogle(options =>
        {
            var googleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
            var googleClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");

            if (string.IsNullOrEmpty(googleClientId) || string.IsNullOrEmpty(googleClientSecret))
            {
                throw new Exception("Google OAuth Client ID or Secret is missing. Set environment variables.");
            }

            options.ClientId = googleClientId;
            options.ClientSecret = googleClientSecret;
            options.CallbackPath = "/signin-google";
        });

        builder.Services.AddAuthorization();

        var app = builder.Build();

        // Configurar pipeline HTTP
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
