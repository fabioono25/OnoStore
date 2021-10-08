using Microsoft.AspNetCore.Mvc;
using OnoStore.WebApp.MVC.Models;
using OnoStore.WebApp.MVC.Services;
using System.Threading.Tasks;

namespace OnoStore.WebApp.MVC.Controllers
{
    public class PedidoController : BaseController
    {
        private readonly IClienteService _clienteService;
        private readonly IPurchaseBffService _comprasBffService;

        public PedidoController(IClienteService clienteService,
            IPurchaseBffService comprasBffService)
        {
            _clienteService = clienteService;
            _comprasBffService = comprasBffService;
        }

        [HttpGet]
        [Route("endereco-de-entrega")]
        public async Task<IActionResult> EnderecoEntrega()
        {
            var carrinho = await _comprasBffService.GetCart();
            if (carrinho.Items.Count == 0) return RedirectToAction("Index", "Cart");

            var endereco = await _clienteService.ObterEndereco();
            var pedido = _comprasBffService.MapearParaPedido(carrinho, endereco);

            return View(pedido);
        }

        [HttpGet]
        [Route("pagamento")]
        public async Task<IActionResult> Pagamento()
        {
            var carrinho = await _comprasBffService.GetCart();
            if (carrinho.Items.Count == 0) return RedirectToAction("Index", "Cart");

            var pedido = _comprasBffService.MapearParaPedido(carrinho, null);

            return View(pedido);
        }

        [HttpPost]
        [Route("finalizar-pedido")]
        public async Task<IActionResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao)
        {
            if (!ModelState.IsValid) return View("Pagamento", _comprasBffService.MapearParaPedido(
                await _comprasBffService.GetCart(), null));

            var retorno = await _comprasBffService.FinalizarPedido(pedidoTransacao);

            if (ResponseHasErrors(retorno))
            {
                var carrinho = await _comprasBffService.GetCart();
                if (carrinho.Items.Count == 0) return RedirectToAction("Index", "Cart");

                var pedidoMap = _comprasBffService.MapearParaPedido(carrinho, null);
                return View("Pagamento", pedidoMap);
            }

            return RedirectToAction("PedidoConcluido");
        }

        [HttpGet]
        [Route("pedido-concluido")]
        public async Task<IActionResult> PedidoConcluido()
        {
            return View("ConfirmacaoPedido", await _comprasBffService.ObterUltimoPedido());
        }

        [HttpGet("meus-pedidos")]
        public async Task<IActionResult> MeusPedidos()
        {
            return View(await _comprasBffService.ObterListaPorClienteId());
        }
    }
}