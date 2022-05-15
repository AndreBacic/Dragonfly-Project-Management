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

        // This method gets called by the runtime. Use this method to add services to the container.
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
                authConfig.AddPolicy("Demo User", policyBuilder =>
                {
                    policyBuilder.RequireClaim(ClaimTypes.Email);
                    policyBuilder.RequireClaim(ClaimTypes.Name);
                    policyBuilder.RequireRole(UserRoles.DEMO_USER);
                });
                authConfig.AddPolicy("Logged_in_user_policy", policyBuilder =>
                {
                    policyBuilder.Combine(authConfig.GetPolicy("Demo User"));
                    policyBuilder.RequireRole(UserRoles.USER); // TODO: make sure this policy doesn't allow demo user roles to pass auth
                });
                authConfig.DefaultPolicy = authConfig.GetPolicy("Logged_in_user_policy");
            });

            services.AddControllersWithViews();
            services.AddRazorPages();

            // Adds globally used usermodel for user info and organization model for organization data.
            services.AddSingleton<IDataAccessor, MongoDBDataAccessor>(); // Just change the implementation to a different DataAccessor as needed.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("Account/Error");
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
                    pattern: "{controller=Account}/{action=Home}");
                endpoints.MapRazorPages();
            });
        }
    }
}
