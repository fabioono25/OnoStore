using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

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

        internal void CalculateValueCart()
        {
            TotalValue = Items.Sum(p => p.CalculateValue());
        }

        internal bool CartExistingItem(CartItem item)
        {
            return Items.Any(p => p.ProductId == item.ProductId);
        }

        internal CartItem GetByProductId(Guid produtoId)
        {
            return Items.FirstOrDefault(p => p.ProductId == produtoId);
        }

        internal void AddItem(CartItem item)
        {
            item.AssociateCart(Id);

            if (CartExistingItem(item))
            {
                var itemExistente = GetByProductId(item.ProductId);
                itemExistente.AddUnits(item.Quantity);

                item = itemExistente;
                Items.Remove(itemExistente);
            }

            Items.Add(item);
            CalculateValueCart();
        }

        internal void UpdateItem(CartItem item)
        {
            item.AssociateCart(Id);

            var itemExistente = GetByProductId(item.ProductId);

            Items.Remove(itemExistente);
            Items.Add(item);

            CalculateValueCart();
        }

        internal void UpdateUnits(CartItem item, int unidades)
        {
            item.UpdateUnits(unidades);
            UpdateItem(item);
        }

        internal void RemoveItem(CartItem item)
        {
            Items.Remove(GetByProductId(item.ProductId));
            CalculateValueCart();
        }

        internal bool IsValid()
        {
            var erros = Items.SelectMany(i => new CartItem.ItemCartValidation().Validate(i).Errors).ToList();
            erros.AddRange(new CustomerCartValidation().Validate(this).Errors);
            ValidationResult = new ValidationResult(erros);

            return ValidationResult.IsValid;
        }

        public class CustomerCartValidation : AbstractValidator<CustomerCart>
        {
            public CustomerCartValidation()
            {
                RuleFor(c => c.CustomerId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Cliente não reconhecido");

                RuleFor(c => c.Items.Count)
                    .GreaterThan(0)
                    .WithMessage("O carrinho não possui itens");

                RuleFor(c => c.TotalValue)
                    .GreaterThan(0)
                    .WithMessage("O valor total do carrinho precisa ser maior que 0");
            }
        }
    }
}