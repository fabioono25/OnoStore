using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OnoStore.WebAPI.Core.User;

namespace OnoStore.Bff.Purchase.Configuration.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();
        }
    }
}