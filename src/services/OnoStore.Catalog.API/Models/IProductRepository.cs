﻿using OnoStore.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnoStore.Catalog.API.Models
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(Guid id);
        Task<List<Product>> GetProductById(string ids);

        void Add(Product product);
        void Update(Product product);
    }
}
