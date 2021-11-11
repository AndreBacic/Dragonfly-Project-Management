using DragonflyDataLibrary;
using DragonflyDataLibrary.DataAccess;
using DragonflyDataLibrary.DataAccess.MongoDB;
using DragonflyDataLibrary.Models;
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
                cookieConfig.LoginPath = "/Account/Home";
                cookieConfig.Cookie.Name = "Dragonfly.UserAuth.cookie"; // TODO: Have an "Our site uses cookies" label for this project and for the Blog.
                cookieConfig.AccessDeniedPath = "/Account/Home";
            });

            services.AddAuthorization(authConfig =>
            {
                authConfig.AddPolicy("Logged_in_user_policy", policyBuilder =>
                {
                    policyBuilder.RequireClaim(ClaimTypes.Email);
                    policyBuilder.RequireClaim(ClaimTypes.Name);
                });
                authConfig.AddPolicy("Logged_into_organization_policy", policyBuilder =>
                {
                    policyBuilder.Combine(authConfig.GetPolicy("Logged_in_user_policy"));
                    policyBuilder.RequireClaim(nameof(OrganizationModel));
                    policyBuilder.RequireClaim(nameof(ProjectModel));
                    policyBuilder.RequireClaim(ClaimTypes.Role);
                });
                authConfig.AddPolicy("Organization_ADMIN_policy", policyBuilder =>
                {
                    policyBuilder.Combine(authConfig.GetPolicy("Logged_into_organization_policy"));
                    string[] roles = { UserPosition.ADMIN.ToString() };
                    policyBuilder.RequireRole(roles); // TODO: Add cookie/something to verify user is logged in to organization as an admin
                });
                authConfig.DefaultPolicy = authConfig.GetPolicy("Logged_into_organization_policy");
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
                app.UseExceptionHandler("/Organization/Error");
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
                    pattern: "{controller=Organization}/{action=OrganizationHome}/{id?}"); // todo: refactor default route (remove the {id?} ?).
                endpoints.MapRazorPages();
            });
        }
    }
}
