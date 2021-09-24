using Microsoft.Extensions.DependencyInjection;
using OnoStore.Customer.API.Data;
using OnoStore.Customer.API.Data.Repository;

namespace OnoStore.Customer.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            //services.AddScoped<IMediatorHandler, MediatorHandler>();
            //services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();

            //services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<CustomerContext>();
        }
    }
}