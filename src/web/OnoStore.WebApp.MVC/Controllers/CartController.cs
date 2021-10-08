using Microsoft.AspNetCore.Mvc;
using OnoStore.WebApp.MVC.Models;
using OnoStore.WebApp.MVC.Services;
using System;
using System.Threading.Tasks;

namespace OnoStore.WebApp.MVC.Controllers
{
    public class CartController : BaseController
    {
        private readonly IPurchaseBffService _purchaseBffService;

        public CartController(IPurchaseBffService purchaseBffService)
        {
            _purchaseBffService = purchaseBffService;
        }

        [Route("cart")]
        public async Task<IActionResult> Index()
        {
            return View(await _purchaseBffService.GetCart());
        }

        [HttpPost]
        [Route("cart/add-item")]
        public async Task<IActionResult> AddItemCart(CartProductItemViewModel itemCartProduct)
        {
            var response = await _purchaseBffService.AddItemCart(itemCartProduct); // avoid external manipulation

            if (ResponseHasErrors(response)) return View("Index", await _purchaseBffService.GetCart());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("cart/update-item")]
        public async Task<IActionResult> UpdateItemCart(Guid productId, int quantity)
        {
            var item = new CartProductItemViewModel { ProductId = productId, Quantity = quantity };

            var response = await _purchaseBffService.UpdateItemCart(productId, item); // avoid external manipulation

            if (ResponseHasErrors(response)) return View("Index", await _purchaseBffService.GetCart());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("cart/delete-item")]
        public async Task<IActionResult> RemoveItemCart(Guid produtoId)
        {
            var response = await _purchaseBffService.RemoveItemCart(produtoId);

            if (ResponseHasErrors(response)) return View("Index", await _purchaseBffService.GetCart());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("carrinho/aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher(string voucherCodigo)
        {
            var resposta = await _purchaseBffService.AplicarVoucherCarrinho(voucherCodigo);

            if (ResponseHasErrors(resposta)) return View("Index", await _purchaseBffService.GetCart());

            return RedirectToAction("Index");
        }

        // validation should be in the backend
        //private void ValidateItemCart(ProductViewModel product, int quantity)
        //{
        //    if (product == null) AddErrorValidation("Product not found!");
        //    if (quantity < 1) AddErrorValidation($"Choose at least one unit of cartProduct {product.Name}");
        //    if (quantity > product.QuantityInventory) AddErrorValidation($"Product {product.Name} has {product.QuantityInventory} units in stock, you selected {quantity}");
        //}
    }
}
