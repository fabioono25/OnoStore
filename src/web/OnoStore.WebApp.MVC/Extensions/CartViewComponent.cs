using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnoStore.WebApp.MVC.Models;
using OnoStore.WebApp.MVC.Services;

namespace OnoStore.WebApp.MVC.Extensions
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;

        public CartViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _cartService.GetCart() ?? new CartViewModel());
        }
    }
}