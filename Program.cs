using LocFarma.Repositories.ADO.SQLServer;
using Microsoft.EntityFrameworkCore;
using LocFarma.Areas.Identity.Data;
using LocFarma.Data;

namespace LocFarma
{
    public class Program
    {
        public static void Main(string[] args)
        {
            UsuarioDAO.LogoutUsuarios();

            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("LocFarmaContextConnection") ?? throw new InvalidOperationException("Connection string 'LocFarmaContextConnection' not found.");

            builder.Services.AddDbContext<LocFarmaContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<LocFarmaUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<LocFarmaContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
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
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();
            
            app.UseSession();

            app.Run();
        }
    }
}