using DragonflyDataLibrary;
using DragonflyDataLibrary.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace DragonflyMVCApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("DragonflyAuth").AddCookie("DragonflyAuth", cookieConfig =>
            {
                cookieConfig.LoginPath = "/Account/Login";
                cookieConfig.Cookie.Name = "Dragonfly.UserAuth.cookie"; // TODO: Have an "Our site uses cookies" label for this project and for the Blog.
                cookieConfig.AccessDeniedPath = "/Account/Login";
            });

            services.AddAuthorization(authConfig =>
            {
                authConfig.AddPolicy("Any_user_policy", policyBuilder =>
                {
                    policyBuilder.RequireClaim(ClaimTypes.Email);
                    policyBuilder.RequireClaim(ClaimTypes.Name);
                });
                authConfig.AddPolicy("Logged_in_user_policy", policyBuilder =>
                {
                    policyBuilder.Combine(authConfig.GetPolicy("Any_user_policy"));
                    policyBuilder.RequireRole(UserRoles.USER);
                });
                authConfig.DefaultPolicy = authConfig.GetPolicy("Logged_in_user_policy");
            });

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSingleton<IDataAccessor, MongoDBDataAccessor>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Home}");
                endpoints.MapRazorPages();
            });
        }
    }
}
