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
        public async Task<CustomerCart> GetCart()
        {
            return await GetCustomerCart() ?? new CustomerCart();
        }

        [HttpPost("cart")]
        public async Task<IActionResult> AddItemCart(CartItem item)
        {
            var cart = await GetCustomerCart();

            if (cart == null)
                ManipulateNewCart(item); // it's a new cart
            else
                ManipulateExistingCart(cart, item);

            if (!ValidOperation()) return CustomResponse();

            await PersistData();
            return CustomResponse();
        }

        [HttpPut("cart/{productId}")]
        public async Task<IActionResult> UpdateCartItem(Guid productId, CartItem item)
        {
            var cart = await GetCustomerCart();
            var itemCart = await GetCartItemValidated(productId, cart, item);
            if (itemCart == null) return CustomResponse();

            cart.UpdateUnits(itemCart, item.Quantity);

            ValidateCart(cart);
            if (!ValidOperation()) return CustomResponse();

            _context.CartItems.Update(itemCart);
            _context.CustomerCart.Update(cart);

            await PersistData();
            return CustomResponse();
        }

        [HttpDelete("cart/{productId}")]
        public async Task<IActionResult> RemoveCartItem(Guid ProductId)
        {
            var cart = await GetCustomerCart();

            var itemCart = await GetCartItemValidated(ProductId, cart);
            if (itemCart == null) return CustomResponse();

            ValidateCart(cart);
            if (!ValidOperation()) return CustomResponse();

            cart.RemoveItem(itemCart);

            _context.CartItems.Remove(itemCart);
            _context.CustomerCart.Update(cart);

            await PersistData();
            return CustomResponse();
        }

        private async Task<CustomerCart> GetCustomerCart()
        {
            return await _context.CustomerCart
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CustomerId == _user.GetUserId());
        }
        private void ManipulateNewCart(CartItem item)
        {
            var cart = new CustomerCart(_user.GetUserId());
            cart.AddItem(item);

            ValidateCart(cart); // must be after adding to the context - otherwise, null object error
            _context.CustomerCart.Add(cart);
        }
        private void ManipulateExistingCart(CustomerCart cart, CartItem item)
        {
            var productItemExistent = cart.CartExistingItem(item);

            cart.AddItem(item);
            ValidateCart(cart);

            if (productItemExistent)
            {
                _context.CartItems.Update(cart.GetItemByProductId(item.ProductId));
            }
            else
            {
                _context.CartItems.Add(item);
            }

            _context.CustomerCart.Update(cart);
        }
        private async Task<CartItem> GetCartItemValidated(Guid productId, CustomerCart cart, CartItem item = null)
        {
            if (item != null && productId != item.ProductId)
            {
                AddErrorProcessing("Item not correspond to the informed one");
                return null;
            }

            if (cart == null)
            {
                AddErrorProcessing("Cart not found");
                return null;
            }

            var itemCart = await _context.CartItems
                .FirstOrDefaultAsync(i => i.CartId == cart.Id && i.ProductId == productId);

            if (itemCart == null || !cart.CartExistingItem(itemCart))
            {
                AddErrorProcessing("Item not in the cart");
                return null;
            }

            return itemCart;
        }
        private async Task PersistData()
        {
            var result = await _context.SaveChangesAsync();
            if (result <= 0) AddErrorProcessing("Not possible to persist in DB");
        }
        private bool ValidateCart(CustomerCart cart)
        {
            if (cart.IsValid()) return true;

            cart.ValidationResult.Errors.ToList().ForEach(e => AddErrorProcessing(e.ErrorMessage));
            return false;
        }
    }
}
