using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnoStore.Core.Mediator;
using OnoStore.Customer.API.Application.Commands;
using OnoStore.Customer.API.Application.Events;
using OnoStore.Customer.API.Data;
using OnoStore.Customer.API.Data.Repository;
using OnoStore.Customer.API.Models;

namespace OnoStore.Customer.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IRequestHandler<RegisterCustomerCommand, ValidationResult>, CustomerCommandHandler>();
            services.AddScoped<IRequestHandler<AdicionarEnderecoCommand, ValidationResult>, CustomerCommandHandler>();

            services.AddScoped<INotificationHandler<RegisteredCustomerEvent>, CustomerEventHandler>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<CustomerContext>();

            // services.AddHostedService<RegisterCustomerIntegrationHandler>(); // Singleton - problems with scoped (MediatorHandler)

        }
    }
}