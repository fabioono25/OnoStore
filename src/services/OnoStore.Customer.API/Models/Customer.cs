using OnoStore.Core.DomainObjects;
using System;

namespace OnoStore.Customer.API.Models
{
    public class Customer : Entity, IAggregateRoot
    {
        public string Nome { get; private set; }
        public Email Email { get; private set; }
        public Cpf Cpf { get; private set; }
        public bool Deleted { get; private set; }
        public Address Address { get; private set; }

        // EF Relation
        protected Customer() { }

        public Customer(Guid id, string nome, string email, string cpf)
        {
            Id = id;
            Nome = nome;
            Email = new Email(email);
            Cpf = new Cpf(cpf);
            Deleted = false;
        }

        public void ChangeEmail(string email)
        {
            Email = new Email(email);
        }

        public void AttributeAddress(Address address)
        {
            Address = address;
        }
    }
}