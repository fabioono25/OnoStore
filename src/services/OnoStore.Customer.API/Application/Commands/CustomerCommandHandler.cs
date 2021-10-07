using FluentValidation.Results;
using MediatR;
using OnoStore.Core.Messages;
using OnoStore.Customer.API.Application.Events;
using OnoStore.Customer.API.Models;
using System.Threading;
using System.Threading.Tasks;

namespace OnoStore.Customer.API.Application.Commands
{
    public class CustomerCommandHandler : CommandHandler, 
        IRequestHandler<RegisterCustomerCommand, ValidationResult>,
        IRequestHandler<AdicionarEnderecoCommand, ValidationResult>
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

            var customerExist = await _customerRepository.GetByCpf(customer.Cpf.Number);

            if (customerExist != null)
            {
                AddError("CPF is in use already.");
                return ValidationResult;
            }

            _customerRepository.Add(customer);

            customer.AddEvent(new RegisteredCustomerEvent(message.Id, message.Name, message.Email, message.Cpf));

            return await PersistData(_customerRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(AdicionarEnderecoCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var endereco = new Address(message.Logradouro, message.Numero, message.Complemento, message.Bairro, message.Cep, message.Cidade, message.Estado, message.ClienteId);
            _customerRepository.AdicionarEndereco(endereco);

            return await PersistData(_customerRepository.UnitOfWork);
        }
    }
}