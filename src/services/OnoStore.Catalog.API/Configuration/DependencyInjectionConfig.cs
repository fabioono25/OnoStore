using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OnoStore.Catalog.API.Data;
using OnoStore.Catalog.API.Data.Repository;
using OnoStore.Catalog.API.Models;

namespace OnoStore.Catalog.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<CatalogContext>();
        }

        public static IApplicationBuilder UseIdentityConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
