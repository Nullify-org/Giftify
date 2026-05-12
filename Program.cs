using Giftify.Data;
using Giftify.Data.Seeder;
using Giftify.Interfaces;
using Giftify.Interfaces.Repositories;
using Giftify.Interfaces.Services;
using Giftify.Models;
using Giftify.Repositories;
using Giftify.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Giftify
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ── Database ──────────────────────────────────────────────────────
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // ── Identity ──────────────────────────────────────────────────────
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit           = false;
                options.Password.RequireLowercase       = false;
                options.Password.RequireUppercase       = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength         = 6;
                options.Password.RequiredUniqueChars    = 0;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // ── External Auth ─────────────────────────────────────────────────
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme       = IdentityConstants.ExternalScheme;
                options.DefaultChallengeScheme    = GoogleDefaults.AuthenticationScheme;
            })
            .AddGoogle(options =>
            {
                options.ClientId     = builder.Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                options.CallbackPath = "/signin-google";
            })
            .AddFacebook(options =>
            {
                options.AppId     = builder.Configuration["Authentication:Facebook:AppId"];
                options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
            });

            // ── Repositories ──────────────────────────────────────────────────
            builder.Services.AddScoped<IUnitOfWork,          UnitOfWork>();
            builder.Services.AddScoped<IProductRepository,   ProductRepository>();
            builder.Services.AddScoped<IOrderRepository,     OrderRepository>();
            builder.Services.AddScoped<ICartRepository,      CartRepository>();
            builder.Services.AddScoped<ICategoryRepository,  CategoryRepository>();
            builder.Services.AddScoped<IOccasionRepository,  OccasionRepository>();

            // ── Services ──────────────────────────────────────────────────────
            builder.Services.AddScoped<IProductService,         ProductService>();
            builder.Services.AddScoped<IOrderService,           OrderService>();
            builder.Services.AddScoped<ICartService,            CartService>();
            builder.Services.AddScoped<ICategoryService,        CategoryService>();
            builder.Services.AddScoped<IOccasionService,        OccasionService>();
            builder.Services.AddScoped<IImageUploadService,     ImageUploadService>();

            // ── Admin Services (new) ──────────────────────────────────────────
            builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
            builder.Services.AddScoped<IAdminUserService,      AdminUserService>();

            // ── MVC + Seeder ──────────────────────────────────────────────────
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ApplicationDbSeeder>();

            var app = builder.Build();

            // ── Middleware ────────────────────────────────────────────────────
            if (!app.Environment.IsDevelopment())
                app.UseExceptionHandler("/Home/Error");

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // ── Routes ────────────────────────────────────────────────────────
            app.MapControllerRoute(
                name:    "admin",
                pattern: "Admin/{controller=AdminDashboard}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name:    "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // ── Auto-migrate + Seed ───────────────────────────────────────────
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (db.Database.GetPendingMigrations().Any())
                    db.Database.Migrate();

                var seeder = scope.ServiceProvider.GetRequiredService<ApplicationDbSeeder>();
                seeder.SeedAsync().GetAwaiter().GetResult();
            }

            app.Run();
        }
    }
}
