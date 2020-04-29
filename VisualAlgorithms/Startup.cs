using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VisualAlgorithms.AppMiddleware;
using VisualAlgorithms.Models;
using VisualAlgorithms.ViewModels;

namespace VisualAlgorithms
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
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddControllersWithViews();

            services.AddScoped<AccessManager, AccessManager>();
            services.AddScoped<TestsManager, TestsManager>();
            services.AddTransient<IValidator<TestQuestionViewModel>, TestQuestionViewModelValidator>();
            services.AddTransient<IValidator<TestAnswer>, TestAnswerValidator>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}"
                    );

                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
            });
        }
    }
}