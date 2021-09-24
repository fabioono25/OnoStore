using System;
using OnoStore.Core.DomainObjects;

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

        public Address(string street, string number, string complement, string neighbor, string cep, string city, string state)
        {
            Street = street;
            Number = number;
            Complement = complement;
            Neighbor = neighbor;
            ZipCode = cep;
            City = city;
            State = state;
        }
    }
}