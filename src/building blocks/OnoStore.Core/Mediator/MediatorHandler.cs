using FluentValidation.Results;
using MediatR;
using OnoStore.Core.Messages;
using System.Threading.Tasks;

namespace OnoStore.Core.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ValidationResult> SendCommand<T>(T command) where T : Command
        {
            return await _mediator.Send(command);
        }

        public async Task PublishEvent<T>(T pEvent) where T : Event
        {
            await _mediator.Publish(pEvent);
        }
    }
}