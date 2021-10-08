using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnoStore.Bff.Purchase.Models;
using OnoStore.Bff.Purchase.Services;
using OnoStore.WebAPI.Core.Controllers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace OnoStore.Bff.Compras.Controllers
{
    [Authorize]
    public class PedidoController : BaseController
    {
        private readonly ICatalogService _catalogoService;
        private readonly ICartService _carrinhoService;
        private readonly IPedidoService _pedidoService;
        private readonly ICustomerService _clienteService;

        public PedidoController(
            ICatalogService catalogoService,
            ICartService carrinhoService,
            IPedidoService pedidoService,
            ICustomerService clienteService)
        {
            _catalogoService = catalogoService;
            _carrinhoService = carrinhoService;
            _pedidoService = pedidoService;
            _clienteService = clienteService;
        }

        [HttpPost]
        [Route("compras/pedido")]
        public async Task<IActionResult> AdicionarPedido(PedidoDTO pedido)
        {
            var carrinho = await _carrinhoService.ObterCarrinho();
            var produtos = await _catalogoService.ObterItens(carrinho.Itens.Select(p => p.ProductId));
            var endereco = await _clienteService.ObterEndereco();

            if (!await ValidarCarrinhoProdutos(carrinho, produtos)) return CustomResponse();

            PopularDadosPedido(carrinho, endereco, pedido);

            return CustomResponse(await _pedidoService.FinalizarPedido(pedido));
        }

        [HttpGet("compras/pedido/ultimo")]
        public async Task<IActionResult> UltimoPedido()
        {
            var pedido = await _pedidoService.ObterUltimoPedido();
            if (pedido is null)
            {
                AddErrorProcessing("Pedido não encontrado!");
                return CustomResponse();
            }

            return CustomResponse(pedido);
        }

        [HttpGet("compras/pedido/lista-cliente")]
        public async Task<IActionResult> ListaPorCliente()
        {
            var pedidos = await _pedidoService.ObterListaPorClienteId();

            return pedidos == null ? NotFound() : CustomResponse(pedidos);
        }

        private async Task<bool> ValidarCarrinhoProdutos(CartDto carrinho, IEnumerable<ProductItemDto> produtos)
        {
            if (carrinho.Itens.Count != produtos.Count())
            {
                var itensIndisponiveis = carrinho.Itens.Select(c => c.ProductId).Except(produtos.Select(p => p.Id)).ToList();

                foreach (var itemId in itensIndisponiveis)
                {
                    var itemCarrinho = carrinho.Itens.FirstOrDefault(c => c.ProductId == itemId);
                    AddErrorProcessing($"O item {itemCarrinho.Name} não está mais disponível no catálogo, o remova do carrinho para prosseguir com a compra");
                }

                return false;
            }

            foreach (var itemCarrinho in carrinho.Itens)
            {
                var produtoCatalogo = produtos.FirstOrDefault(p => p.Id == itemCarrinho.ProductId);

                if (produtoCatalogo.Value != itemCarrinho.Value)
                {
                    var msgErro = $"O produto {itemCarrinho.Name} mudou de Value (de: " +
                                  $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", itemCarrinho.Value)} para: " +
                                  $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", produtoCatalogo.Value)}) desde que foi adicionado ao carrinho.";

                    AddErrorProcessing(msgErro);

                    var responseRemover = await _carrinhoService.RemoverItemCarrinho(itemCarrinho.ProductId);
                    if (ResponseHasErrors(responseRemover))
                    {
                        AddErrorProcessing($"Não foi possível remover automaticamente o produto {itemCarrinho.Name} do seu carrinho, _" +
                                           "remova e adicione novamente caso ainda deseje comprar este item");
                        return false;
                    }

                    itemCarrinho.Value = produtoCatalogo.Value;
                    var responseAdicionar = await _carrinhoService.AdicionarItemCarrinho(itemCarrinho);

                    if (ResponseHasErrors(responseAdicionar))
                    {
                        AddErrorProcessing($"Não foi possível atualizar automaticamente o produto {itemCarrinho.Name} do seu carrinho, _" +
                                           "adicione novamente caso ainda deseje comprar este item");
                        return false;
                    }

                    CleanErrorProcessing();
                    AddErrorProcessing(msgErro + " Atualizamos o Value em seu carrinho, realize a conferência do pedido e se preferir remova o produto");

                    return false;
                }
            }

            return true;
        }

        private void PopularDadosPedido(CartDto carrinho, EnderecoDTO endereco, PedidoDTO pedido)
        {
            pedido.VoucherCodigo = carrinho.Voucher?.Codigo;
            pedido.VoucherUtilizado = carrinho.VoucherUtilizado;
            pedido.ValorTotal = carrinho.ValorTotal;
            pedido.Desconto = carrinho.Desconto;
            pedido.PedidoItems = carrinho.Itens;

            pedido.Endereco = endereco;
        }
    }
}
