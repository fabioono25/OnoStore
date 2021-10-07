using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSE.Core.Messages.Integration;
using OnoStore.Cart.API.Data;
using OnoStore.MessageBus;

namespace OnoStore.Cart.API.Services
{
    public class CarrinhoIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public CarrinhoIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private void SetSubscribers()
        {
            _bus.SubscribeAsync<PedidoRealizadoIntegrationEvent>("PedidoRealizado", async request =>
                await ApagarCarrinho(request));
        }

        private async Task ApagarCarrinho(PedidoRealizadoIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope(); // instanciar objetos que sao resolvidos em modo Scoped in a Singleton mode
            var context = scope.ServiceProvider.GetRequiredService<CartContext>();

            var carrinho = await context.CustomerCart
                .FirstOrDefaultAsync(c => c.CustomerId == message.ClienteId);

            if (carrinho != null)
            {
                context.CustomerCart.Remove(carrinho);
                await context.SaveChangesAsync();
            }
        }
    }
}