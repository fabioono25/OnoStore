using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Bff.Compras.Services.gRPC;
using OnoStore.Bff.Purchase.Models;
using OnoStore.Bff.Purchase.Services;
using OnoStore.WebAPI.Core.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnoStore.Bff.Purchase.Controllers
{
    [Authorize]
    public class CartController : BaseController
    {
        private readonly ICartService _carrinhoService;
        private readonly ICatalogService _catalogoService;
        private readonly IPedidoService _pedidoService;
        private readonly ICarrinhoGrpcService _carrinhoGrpcService;

        public CartController(
            ICartService carrinhoService,
            ICatalogService catalogoService,
            IPedidoService pedidoService,
            ICarrinhoGrpcService carrinhoGrpcService)
        {
            _carrinhoService = carrinhoService;
            _catalogoService = catalogoService;
            _pedidoService = pedidoService;
            _carrinhoGrpcService = carrinhoGrpcService;
        }

        [HttpGet]
        [Route("compras/carrinho")]
        public async Task<IActionResult> Index()
        {
            return CustomResponse(await _carrinhoGrpcService.ObterCarrinho());
        }

        [HttpGet]
        [Route("compras/carrinho-quantidade")]
        public async Task<int> ObterQuantidadeCarrinho()
        {
            var quantidade = await _carrinhoGrpcService.ObterCarrinho();
            return quantidade?.Itens.Sum(i => i.Quantity) ?? 0;
        }

        [HttpPost]
        [Route("compras/carrinho/items")]
        public async Task<IActionResult> AdicionarItemCarrinho(ProductItemCartDto itemProduto)
        {
            var product = await _catalogoService.ObterPorId(itemProduto.ProductId);

            await ValidarItemCarrinho(product, itemProduto.Quantity, true);
            if (!ValidOperation()) return CustomResponse();

            itemProduto.Name = product.Name;
            itemProduto.Value = product.Value;
            itemProduto.Image = product.Image;

            var resposta = await _carrinhoService.AdicionarItemCarrinho(itemProduto);

            return CustomResponse(resposta);
        }

        [HttpPut]
        [Route("compras/carrinho/items/{produtoId}")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, ProductItemCartDto itemProduto)
        {
            var produto = await _catalogoService.ObterPorId(produtoId);

            await ValidarItemCarrinho(produto, itemProduto.Quantity);
            if (!ValidOperation()) return CustomResponse();

            var resposta = await _carrinhoService.AtualizarItemCarrinho(produtoId, itemProduto);

            return CustomResponse(resposta);
        }

        [HttpDelete]
        [Route("compras/carrinho/items/{produtoId}")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var produto = await _catalogoService.ObterPorId(produtoId);

            if (produto == null)
            {
                AddErrorProcessing("Produto inexistente!");
                return CustomResponse();
            }

            var resposta = await _carrinhoService.RemoverItemCarrinho(produtoId);

            return CustomResponse(resposta);
        }

        [HttpPost]
        [Route("compras/carrinho/aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher([FromBody] string voucherCodigo)
        {
            var voucher = await _pedidoService.ObterVoucherPorCodigo(voucherCodigo);
            if (voucher is null)
            {
                AddErrorProcessing("Voucher inválido ou não encontrado!");
                return CustomResponse();
            }

            var resposta = await _carrinhoService.AplicarVoucherCarrinho(voucher);

            return CustomResponse(resposta);
        }

        private async Task ValidarItemCarrinho(ProductItemDto produto, int quantidade, bool adicionarProduto = false)
        {
            if (produto == null) AddErrorProcessing("Produto inexistente!");
            if (quantidade < 1) AddErrorProcessing($"Escolha ao menos uma unidade do produto {produto.Name}");

            var carrinho = await _carrinhoService.ObterCarrinho();
            var itemCarrinho = carrinho.Itens.FirstOrDefault(p => p.ProductId == produto.Id);

            if (itemCarrinho != null && adicionarProduto && itemCarrinho.Quantity + quantidade > produto.QuantityAvailable)
            {
                AddErrorProcessing($"O produto {produto.Name} possui {produto.QuantityAvailable} unidades em estoque, você selecionou {quantidade}");
                return;
            }

            if (quantidade > produto.QuantityAvailable) AddErrorProcessing($"O produto {produto.Name} possui {produto.QuantityAvailable} unidades em estoque, você selecionou {quantidade}");
        }
    }
}
