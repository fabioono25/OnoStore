using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnoStore.WebAPI.Core.Controllers;

namespace OnoStore.Bff.Purchase.Controllers
{
    [Authorize]
    public class CartController : BaseController
    {
        [HttpGet]
        [Route("compras/carrinho")]
        public async Task<IActionResult> Index()
        {
            return CustomResponse();
        }

        [HttpGet]
        [Route("compras/carrinho-quantidade")]
        public async Task<IActionResult> ObterQuantidadeCarrinho()
        {
            return CustomResponse();
        }

        [HttpPost]
        [Route("compras/carrinho/items")]
        public async Task<IActionResult> AdicionarItemCarrinho()
        {
            return CustomResponse();
        }

        [HttpPut]
        [Route("compras/carrinho/items/{produtoId}")]
        public async Task<IActionResult> AtualizarItemCarrinho()
        {
            return CustomResponse();
        }

        [HttpDelete]
        [Route("compras/carrinho/items/{produtoId}")]
        public async Task<IActionResult> RemoverItemCarrinho()
        {
            return CustomResponse();
        }
    }
}
