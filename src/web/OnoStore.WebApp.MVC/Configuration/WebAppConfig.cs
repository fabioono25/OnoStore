using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnoStore.WebApp.MVC.Extensions;

namespace OnoStore.WebApp.MVC.Configuration
{
    public static class WebAppConfig
    {
        public static void AddMvcConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();
            services.Configure<AppSettings>(configuration);
        }

        public static void UseMvcConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/error/500");                   // using the exception middleware - probably error in the server
            //    app.UseStatusCodePagesWithRedirects("/error/{0}");       // using the exception middleware - treated errors
            //    app.UseHsts();
            //}

            app.UseExceptionHandler("/error/500");                   // using the exception middleware - probably error in the server
            app.UseStatusCodePagesWithRedirects("/error/{0}");       // using the exception middleware - treated errors
            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityConfiguration();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Index}/{action=Index}/{id?}");
            });
        }
    }
}
