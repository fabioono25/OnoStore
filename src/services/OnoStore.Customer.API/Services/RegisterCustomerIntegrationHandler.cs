using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnoStore.Core.Mediator;
using OnoStore.Core.Messages.Integration;
using OnoStore.Customer.API.Application.Commands;
using OnoStore.MessageBus;

namespace OnoStore.Customer.API.Services
{
    // it's a singleton service (working as a pipeline of ASP.Net)
    public class RegisterCustomerIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public RegisterCustomerIntegrationHandler(
                            IServiceProvider serviceProvider,
                            IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        private void SetRespond()
        {
            _bus.RespondAsync<UserRegisteredIntegrationEvent, ResponseMessage>(async request =>
                await RegisterCustomer(request));

            _bus.AdvancedBus.Connected += OnConnect;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetRespond();
            return Task.CompletedTask;
        }

        private void OnConnect(object s, EventArgs e)
        {
            SetRespond();
        }

        private async Task<ResponseMessage> RegisterCustomer(UserRegisteredIntegrationEvent message)
        {
            var customerCommand = new RegisterCustomerCommand(message.Id, message.Nome, message.Email, message.Cpf);

            // workaround to inject a scoped object into a Singleton service: ServiceLocator
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
            var success = await mediator.SendCommand(customerCommand);
            
            return new ResponseMessage(success);
        }
    }
}