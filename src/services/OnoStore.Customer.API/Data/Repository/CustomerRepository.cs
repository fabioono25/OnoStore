using Microsoft.EntityFrameworkCore;
using OnoStore.Core.Data;
using OnoStore.Customer.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnoStore.Customer.API.Data.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerContext _context;

        public CustomerRepository(CustomerContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Models.Customer>> GetAll()
        {
            return await _context.Customers.AsNoTracking().ToListAsync();
        }

        public Task<Models.Customer> GetByCpf(string cpf)
        {
            return _context.Customers.FirstOrDefaultAsync(c => c.Cpf.Number == cpf);
        }

        public void Add(Models.Customer customer)
        {
            _context.Customers.Add(customer);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}