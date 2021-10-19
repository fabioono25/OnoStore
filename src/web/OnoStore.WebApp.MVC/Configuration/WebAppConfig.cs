using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnoStore.WebApp.MVC.Extensions;
using System.Globalization;

namespace OnoStore.WebApp.MVC.Configuration
{
    public static class WebAppConfig
    {
        public static void AddMvcConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();

            // if you need to scale into multiple clusters and avoid loosing the authentication key
            // different keys generated per container
            services.AddDataProtection()
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo(@"/var/data_protection_keys/"))
                .SetApplicationName("OnoStore");

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });


            services.Configure<AppSettings>(configuration);
        }

        public static void UseMvcConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();
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

            var supportedCultures = new[] { new CultureInfo("en-IE") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-IE"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

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
