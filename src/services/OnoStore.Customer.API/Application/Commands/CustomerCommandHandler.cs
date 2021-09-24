using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using NSE.Clientes.API.Application.Events;
using OnoStore.Core.Messages;
using OnoStore.Customer.API.Data.Repository;

namespace OnoStore.Customer.API.Application.Commands
{
    public class CustomerCommandHandler : CommandHandler //, IRequestHandler<RegisterCustomerCommand, ValidationResult>
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        
        public async Task<ValidationResult> Handle(RegisterCustomerCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var customer = new Models.Customer(message.Id, message.Name, message.Email, message.Cpf);

            //var clienteExistente = await _customerRepository.ObterPorCpf(cliente.Cpf.Numero);

            //if (clienteExistente != null)
            //{
            //    AdicionarErro("Este CPF já está em uso.");
            //    return ValidationResult;
            //}

            //_customerRepository.Adicionar(cliente);

            //cliente.AdicionarEvento(new ClienteRegistradoEvent(message.Id, message.Nome, message.Email, message.Cpf));

            //return await PersistirDados(_customerRepository.UnitOfWork);
            return null;
        }
    }
}