using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnoStore.WebApp.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using OnoStore.WebApp.MVC.Models;

namespace OnoStore.WebApp.MVC.Controllers
{
    public class CartController: BaseController
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
            var product = await _catalogoService.GetById(itemProduct.ProductId);

            ValidateItemCart(product, itemProduct.Quantity);
            if (!IsValidOperation()) return View("Index", await _cartService.GetCart());

            itemProduct.Name = product.Name;
            itemProduct.Value = product.Value;
            itemProduct.Image = product.Image;

            var resposta = await _cartService.AddItemCart(itemProduct);

            if (ResponseHasErrors(resposta)) return View("Index", await _cartService.GetCart());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("cart/update-item")]
        public async Task<IActionResult> UpdateItemCart(Guid productId, int quantity)
        {
            var produto = await _catalogoService.GetById(productId);

            ValidateItemCart(produto, quantity);
            if (!IsValidOperation()) return View("Index", await _cartService.GetCart());

            var itemProduto = new ProductItemViewModel { ProductId = productId, Quantity = quantity };
            var resposta = await _cartService.UpdateItemCart(productId, itemProduto);

            if (ResponseHasErrors(resposta)) return View("Index", await _cartService.GetCart());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("cart/delete-item")]
        public async Task<IActionResult> RemoveItemCart(Guid produtoId)
        {
            var produto = await _catalogoService.GetById(produtoId);

            if (produto == null)
            {
                AddErrorValidation("Produto inexistente!");
                return View("Index", await _cartService.GetCart());
            }

            var resposta = await _cartService.RemoveItemCart(produtoId);

            if (ResponseHasErrors(resposta)) return View("Index", await _cartService.GetCart());

            return RedirectToAction("Index");
        }

        private void ValidateItemCart(ProductViewModel product, int quantity)
        {
            if (product == null) AddErrorValidation("Produto inexistente!");
            if (quantity < 1) AddErrorValidation($"Escolha ao menos uma unidade do product {product.Name}");
            if (quantity > product.QuantityInventory) AddErrorValidation($"O product {product.Name} possui {product.QuantityInventory} unidades em estoque, você selecionou {quantity}");
        }
    }
}
