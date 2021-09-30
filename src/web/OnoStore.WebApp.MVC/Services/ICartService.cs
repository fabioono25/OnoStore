using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnoStore.WebApp.MVC.Models;

namespace OnoStore.WebApp.MVC.Services
{
    public interface ICartService
    {
        Task<CartViewModel> GetCart();
        Task<ResponseResult> AddItemCart(ProductItemViewModel product);
        Task<ResponseResult> UpdateItemCart(Guid produtoId, ProductItemViewModel produto);
        Task<ResponseResult> RemoveItemCart(Guid produtoId);
    }
}
