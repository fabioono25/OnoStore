using OnoStore.WebApp.MVC.Models;
using System;
using System.Threading.Tasks;
using OnoStore.Core.MVC.Models;

namespace OnoStore.WebApp.MVC.Services
{
    public interface IPurchaseBffService
    {
        Task<CartViewModel> GetCart();
        Task<int> GetQuantityCart();
        Task<ResponseResult> AddItemCart(CartProductItemViewModel cartProduct);
        Task<ResponseResult> UpdateItemCart(Guid produtoId, CartProductItemViewModel produto);
        Task<ResponseResult> RemoveItemCart(Guid produtoId);
        Task<ResponseResult> AplicarVoucherCarrinho(string voucher);
    }
}
