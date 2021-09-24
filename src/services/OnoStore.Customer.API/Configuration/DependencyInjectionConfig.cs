using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnoStore.Core.Mediator;
using OnoStore.Customer.API.Application.Commands;
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

            //services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<CustomerContext>();
        }
    }
}