using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnoStore.Core.Utils;

namespace OnoStore.Customer.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            //services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
            //    .AddHostedService<RegistroClienteIntegrationHandler>();
        }
    }
}