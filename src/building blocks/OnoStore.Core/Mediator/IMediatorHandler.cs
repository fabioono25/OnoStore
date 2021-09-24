using System.Threading.Tasks;
using FluentValidation.Results;
using OnoStore.Core.Messages;

namespace OnoStore.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T pEvent) where T : Event;
        Task<ValidationResult> SendCommand<T>(T command) where T : Command;
    }
}