using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnoStore.Core.Data;
using OnoStore.Core.DomainObjects;

namespace OnoStore.Catalog.API.Models
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(Guid id);

        void Add(Product product);
        void Update(Product product);
    }
}
