using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnoStore.Catalog.API.Models;
using OnoStore.WebAPI.Core.Controllers;
using OnoStore.WebAPI.Core.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnoStore.Catalog.API.Controllers
{
    [ApiController]
    [Authorize]
    public class CatalogController : BaseController
    {
        private readonly IProductRepository _productRepository;

        public CatalogController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [AllowAnonymous]
        [HttpGet("catalog/products")]
        public async Task<IEnumerable<Product>> Index()
        {
            return await _productRepository.GetAll();
        }

        [ClaimsAuthorize("Catalog", "Read")]
        [HttpGet("catalog/products/{id}")]
        public async Task<Product> ProductDetail(Guid id)
        {
            return await _productRepository.GetById(id);
        }

        [HttpGet("catalogo/produtos/lista/{ids}")]
        public async Task<IEnumerable<Product>> GetProductsById(string ids)
        {
            return await _productRepository.GetProductById(ids);
        }
    }
}
