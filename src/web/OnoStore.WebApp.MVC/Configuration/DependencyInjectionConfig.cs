using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OnoStore.WebApp.MVC.Extensions;
using OnoStore.WebApp.MVC.Services;
using OnoStore.WebApp.MVC.Services.Handlers;

namespace OnoStore.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>(); // not enough - we need to listen in some service

            services.AddHttpClient<IAutenticationService, AuthenticationService>(); // HTTP service 

            services.AddHttpClient<ICatalogService, CatalogService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>(); // HTTP service 

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();
        }
    }
}
