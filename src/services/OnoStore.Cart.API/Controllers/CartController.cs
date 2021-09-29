using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnoStore.Cart.API.Data;
using OnoStore.Cart.API.Model;
using OnoStore.WebAPI.Core.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using OnoStore.WebAPI.Core.User;

namespace OnoStore.Cart.API.Controllers
{
    [Authorize]
    public class CartController : BaseController
    {
        private readonly IAspNetUser _user;
        private readonly CartContext _context;

        public CartController(IAspNetUser user, CartContext context)
        {
            _user = user;
            _context = context;
        }

        [HttpGet("cart")]
        public async Task<CustomerCart> ObterCarrinho()
        {
            return await ObterCustomerCart() ?? new CustomerCart();
        }

        [HttpPost("carrinho")]
        public async Task<IActionResult> AdicionarItemCarrinho(CartItem item)
        {
            var carrinho = await ObterCustomerCart();

            if (carrinho == null)
                ManipulateNewCart(item);
            else
                ManipularCarrinhoExistente(carrinho, item);

            if (!ValidOperation()) return CustomResponse();

            await PersistirDados();
            return CustomResponse();
        }

        [HttpPut("carrinho/{ProductId}")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid ProductId, CartItem item)
        {
            var carrinho = await ObterCustomerCart();
            var itemCarrinho = await ObterItemCarrinhoValidado(ProductId, carrinho, item);
            if (itemCarrinho == null) return CustomResponse();

            carrinho.UpdateUnits(itemCarrinho, item.Quantity);

            ValidarCarrinho(carrinho);
            if (!ValidOperation()) return CustomResponse();

            _context.CartItems.Update(itemCarrinho);
            _context.CustomerCart.Update(carrinho);

            await PersistirDados();
            return CustomResponse();
        }

        [HttpDelete("carrinho/{ProductId}")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid ProductId)
        {
            var carrinho = await ObterCustomerCart();

            var itemCarrinho = await ObterItemCarrinhoValidado(ProductId, carrinho);
            if (itemCarrinho == null) return CustomResponse();

            ValidarCarrinho(carrinho);
            if (!ValidOperation()) return CustomResponse();

            carrinho.RemoveItem(itemCarrinho);

            _context.CartItems.Remove(itemCarrinho);
            _context.CustomerCart.Update(carrinho);

            await PersistirDados();
            return CustomResponse();
        }

        private async Task<CustomerCart> ObterCustomerCart()
        {
            return await _context.CustomerCart
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CustomerId == _user.GetUserId());
        }
        private void ManipulateNewCart(CartItem item)
        {
            var carrinho = new CustomerCart(_user.GetUserId());
            carrinho.AddItem(item);

            ValidarCarrinho(carrinho);
            _context.CustomerCart.Add(carrinho);
        }
        private void ManipularCarrinhoExistente(CustomerCart carrinho, CartItem item)
        {
            var produtoItemExistente = carrinho.CartExistingItem(item);

            carrinho.AddItem(item);
            ValidarCarrinho(carrinho);

            if (produtoItemExistente)
            {
                _context.CartItems.Update(carrinho.GetByProductId(item.ProductId));
            }
            else
            {
                _context.CartItems.Add(item);
            }

            _context.CustomerCart.Update(carrinho);
        }
        private async Task<CartItem> ObterItemCarrinhoValidado(Guid ProductId, CustomerCart carrinho, CartItem item = null)
        {
            if (item != null && ProductId != item.ProductId)
            {
                AddErrorProcessing("O item não corresponde ao informado");
                return null;
            }

            if (carrinho == null)
            {
                AddErrorProcessing("Carrinho não encontrado");
                return null;
            }

            var itemCarrinho = await _context.CartItems
                .FirstOrDefaultAsync(i => i.CartId == carrinho.Id && i.ProductId == ProductId);

            if (itemCarrinho == null || !carrinho.CartExistingItem(itemCarrinho))
            {
                AddErrorProcessing("O item não está no carrinho");
                return null;
            }

            return itemCarrinho;
        }
        private async Task PersistirDados()
        {
            var result = await _context.SaveChangesAsync();
            if (result <= 0) AddErrorProcessing("Não foi possível persistir os dados no banco");
        }
        private bool ValidarCarrinho(CustomerCart carrinho)
        {
            if (carrinho.IsValid()) return true;

            carrinho.ValidationResult.Errors.ToList().ForEach(e => AddErrorProcessing(e.ErrorMessage));
            return false;
        }
    }
}
