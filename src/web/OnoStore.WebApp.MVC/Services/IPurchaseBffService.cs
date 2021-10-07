using OnoStore.WebApp.MVC.Models;
using System;
using System.Collections.Generic;
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

        PedidoTransacaoViewModel MapearParaPedido(CartViewModel carrinho, EnderecoViewModel endereco);
        Task<ResponseResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao);
        Task<PedidoViewModel> ObterUltimoPedido();
        Task<IEnumerable<PedidoViewModel>> ObterListaPorClienteId();
    }
}
