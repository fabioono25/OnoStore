using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnoStore.Core.Data;

namespace OnoStore.Customer.API.Models
{
    public interface ICustomerRepository: IRepository<Customer>
    {
        Task<IEnumerable<Models.Customer>> GetAll();
        Task<Models.Customer> GetByCpf(string cpf);
        void Add(Models.Customer customer);

    }
}
