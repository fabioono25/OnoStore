using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnoStore.Core.Data;

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

        public async Task<IEnumerable<Models.Customer>> ObterTodos()
        {
            return await _context.Customers.AsNoTracking().ToListAsync();
        }

        public Task<Models.Customer> ObterPorCpf(string cpf)
        {
            return _context.Customers.FirstOrDefaultAsync(c => c.Cpf.Number == cpf);
        }

        public void Adicionar(Models.Customer cliente)
        {
            _context.Customers.Add(cliente);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}