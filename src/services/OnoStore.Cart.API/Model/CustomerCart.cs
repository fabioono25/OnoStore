using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnoStore.Cart.API.Model
{
    public class CustomerCart
    {
        internal const int MAX_QUANTIDADE_ITEM = 5;

        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalValue { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public ValidationResult ValidationResult { get; set; }

        public CustomerCart(Guid customerId)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
        }

        public CustomerCart() { }

        internal void CalculateTotalValueCart()
        {
            TotalValue = Items.Sum(p => p.CalculateValue());
        }

        internal bool CartExistingItem(CartItem item)
        {
            return Items.Any(p => p.ProductId == item.ProductId);
        }

        internal CartItem GetItemByProductId(Guid productId)
        {
            return Items.FirstOrDefault(p => p.ProductId == productId);
        }

        internal void AddItem(CartItem item)
        {
            //if (!item.IsValid()) return; - it will be validated at the end

            item.AssociateCart(Id);

            if (CartExistingItem(item)) // the item is already in the cart
            {
                var existingItem = GetItemByProductId(item.ProductId);
                existingItem.AddUnits(item.Quantity);

                item = existingItem;
                Items.Remove(existingItem);
            }

            Items.Add(item);
            CalculateTotalValueCart();
        }

        internal void UpdateItem(CartItem item)
        {
            item.AssociateCart(Id);

            var existingItem = GetItemByProductId(item.ProductId);

            Items.Remove(existingItem);
            Items.Add(item);

            CalculateTotalValueCart();
        }

        internal void UpdateUnits(CartItem item, int unidades)
        {
            item.UpdateUnits(unidades);
            UpdateItem(item);
        }

        internal void RemoveItem(CartItem item)
        {
            Items.Remove(GetItemByProductId(item.ProductId));
            CalculateTotalValueCart();
        }

        internal bool IsValid()
        {
            var errors = Items.SelectMany(i => new CartItem.ItemCartValidation().Validate(i).Errors).ToList();
            errors.AddRange(new CustomerCartValidation().Validate(this).Errors);
            ValidationResult = new ValidationResult(errors);

            return ValidationResult.IsValid;
        }

        public class CustomerCartValidation : AbstractValidator<CustomerCart>
        {
            public CustomerCartValidation()
            {
                RuleFor(c => c.CustomerId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Customer not found");

                RuleFor(c => c.Items.Count)
                    .GreaterThan(0)
                    .WithMessage("Cart without items");

                RuleFor(c => c.TotalValue)
                    .GreaterThan(0)
                    .WithMessage("Total value of the cart must be higher than 0");
            }
        }
    }
}