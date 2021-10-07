using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnoStore.Cart.API.Services;
using OnoStore.Core.Utils;
using OnoStore.MessageBus;

namespace OnoStore.Cart.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                .AddHostedService<CarrinhoIntegrationHandler>();
        }
    }
}