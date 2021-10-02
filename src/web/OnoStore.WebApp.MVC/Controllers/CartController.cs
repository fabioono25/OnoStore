using Microsoft.AspNetCore.Mvc;
using OnoStore.WebApp.MVC.Models;
using OnoStore.WebApp.MVC.Services;
using System;
using System.Threading.Tasks;

namespace OnoStore.WebApp.MVC.Controllers
{
    public class CartController : BaseController
    {
        private readonly ICartService _cartService;
        private readonly ICatalogService _catalogoService;

        public CartController(ICartService cartService,
            ICatalogService catalogoService)
        {
            _cartService = cartService;
            _catalogoService = catalogoService;
        }

        [Route("cart")]
        public async Task<IActionResult> Index()
        {
            return View(await _cartService.GetCart());
        }

        [HttpPost]
        [Route("cart/add-item")]
        public async Task<IActionResult> AddItemCart(ProductItemViewModel itemProduct)
        {
            var product = await _catalogoService.GetById(itemProduct.ProductId); // avoid external manipulation

            ValidateItemCart(product, itemProduct.Quantity);
            if (!IsValidOperation()) return View("Index", await _cartService.GetCart());

            itemProduct.Name = product.Name;
            itemProduct.Value = product.Value;
            itemProduct.Image = product.Image;

            var response = await _cartService.AddItemCart(itemProduct);

            if (ResponseHasErrors(response)) return View("Index", await _cartService.GetCart());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("cart/update-item")]
        public async Task<IActionResult> UpdateItemCart(Guid productId, int quantity)
        {
            var product = await _catalogoService.GetById(productId);

            ValidateItemCart(product, quantity);
            if (!IsValidOperation()) return View("Index", await _cartService.GetCart());

            var productItem = new ProductItemViewModel { ProductId = productId, Quantity = quantity };
            var response = await _cartService.UpdateItemCart(productId, productItem);

            if (ResponseHasErrors(response)) return View("Index", await _cartService.GetCart());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("cart/delete-item")]
        public async Task<IActionResult> RemoveItemCart(Guid produtoId)
        {
            var product = await _catalogoService.GetById(produtoId);

            if (product == null)
            {
                AddErrorValidation("Produto inexistente!");
                return View("Index", await _cartService.GetCart());
            }

            var response = await _cartService.RemoveItemCart(produtoId);

            if (ResponseHasErrors(response)) return View("Index", await _cartService.GetCart());

            return RedirectToAction("Index");
        }

        private void ValidateItemCart(ProductViewModel product, int quantity)
        {
            if (product == null) AddErrorValidation("Product not found!");
            if (quantity < 1) AddErrorValidation($"Choose at least one unit of product {product.Name}");
            if (quantity > product.QuantityInventory) AddErrorValidation($"Product {product.Name} has {product.QuantityInventory} units in stock, you selected {quantity}");
        }
    }
}
