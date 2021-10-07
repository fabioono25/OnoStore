using System;
using OnoStore.Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnoStore.Customer.API.Models
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<IEnumerable<Models.Customer>> GetAll();
        Task<Models.Customer> GetByCpf(string cpf);
        void Add(Models.Customer customer);
        Task<Address> ObterEnderecoPorId(Guid id);
        void AdicionarEndereco(Address endereco);
    }
}
