using System;
using FluentValidation;
using OnoStore.Core.Messages;

namespace OnoStore.Customer.API.Application.Commands
{
    public class RegisterCustomerCommand : Command
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; }

        public RegisterCustomerCommand(Guid id, string nome, string email, string cpf)
        {
            AggregateId = id;
            Id = id;
            Name = nome;
            Email = email;
            Cpf = cpf;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterCustomerValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class RegisterCustomerValidation : AbstractValidator<RegisterCustomerCommand>
        {
            public RegisterCustomerValidation()
            {
                RuleFor(c => c.Id)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do cliente inválido");

                RuleFor(c => c.Name)
                    .NotEmpty()
                    .WithMessage("O nome do cliente não foi informado");

                RuleFor(c => c.Cpf)
                    .Must(HasValidCpf)
                    .WithMessage("O CPF informado não é válido.");

                RuleFor(c => c.Email)
                    .Must(HasValidEmail)
                    .WithMessage("O e-mail informado não é válido.");
            }

            protected static bool HasValidCpf(string cpf)
            {
                return Core.DomainObjects.Cpf.IsValid(cpf);
            }

            protected static bool HasValidEmail(string email)
            {
                return Core.DomainObjects.Email.IsValid(email);
            }
        }
    }
}