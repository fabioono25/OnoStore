using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace OnoStore.Customer.API.Application.Events
{
    public class CustomerEventHandler : INotificationHandler<RegisteredCustomerEvent>
    {
        public Task Handle(RegisteredCustomerEvent notification, CancellationToken cancellationToken)
        {
            // Enviar evento de confirmação (email via infra class?)
            return Task.CompletedTask;
        }
    }
}