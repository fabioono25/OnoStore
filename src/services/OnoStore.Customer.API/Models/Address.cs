using OnoStore.Core.DomainObjects;
using System;

namespace OnoStore.Customer.API.Models
{
    public class Address : Entity
    {
        public string Street { get; private set; }
        public string Number { get; private set; }
        public string Complement { get; private set; }
        public string Neighbor { get; private set; }
        public string ZipCode { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public Guid CustomerId { get; private set; }

        // EF Relation
        public Customer Customer { get; protected set; }

        public Address()
        {
        }

        public Address(string logradouro, string numero, string complemento, string bairro, string cep, string cidade, string estado, Guid clienteId)
        {
            Street = logradouro;
            Number = numero;
            Complement = numero;
            Neighbor = bairro;
            ZipCode = cep;
            City = cidade;
            State = estado;
            CustomerId = clienteId;
        }
    }
}