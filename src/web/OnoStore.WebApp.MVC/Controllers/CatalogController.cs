using Microsoft.AspNetCore.Mvc;
using OnoStore.WebApp.MVC.Services;
using System;
using System.Threading.Tasks;

namespace OnoStore.WebApp.MVC.Controllers
{
    public class CatalogController : BaseController
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        [Route("")]
        [Route("showcase")]
        public async Task<IActionResult> Index([FromQuery] int ps = 8, [FromQuery] int page = 1, [FromQuery] string q = null)
        {
            var products = await _catalogService.GetAll(ps, page, q);
            ViewBag.Pesquisa = q; // persist info
            products.ReferenceAction = "Index";

            return View(products);
        }

        [HttpGet]
        [Route("product-detail/{id}")]
        public async Task<IActionResult> ProductDetail(Guid id)
        {
            var product = await _catalogService.GetById(id);

            return View(product);
        }
    }
}
