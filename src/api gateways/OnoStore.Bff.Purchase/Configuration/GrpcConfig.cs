using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.Bff.Compras.Services.gRPC;
using OnoStore.Cart.API.Services.gRPC;

namespace NSE.Bff.Compras.Configuration
{
    public static class GrpcConfig
    {
        public static void ConfigureGrpcServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<GrpcServiceInterceptor>();

            services.AddScoped<ICarrinhoGrpcService, CarrinhoGrpcService>();

            services.AddGrpcClient<CartOrders.CartOrdersClient>(options =>
            {
                options.Address = new Uri(configuration["CarrinhoUrl"]);
            }).AddInterceptor<GrpcServiceInterceptor>();
        }
    }
}