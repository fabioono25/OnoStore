using Microsoft.AspNetCore.Mvc;
using OnoStore.WebApp.MVC.Services;
using System.Threading.Tasks;

namespace OnoStore.WebApp.MVC.Extensions
{
    public class CartViewComponent : ViewComponent
    {
        private readonly IPurchaseBffService _purchaseBffService;

        public CartViewComponent(IPurchaseBffService purchaseBffService)
        {
            _purchaseBffService = purchaseBffService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //return View(await _purchaseBffService.GetCart() ?? new CartViewModel());
            return View(await _purchaseBffService.GetQuantityCart());
        }
    }
}