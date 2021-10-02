using FluentValidation;
using System;
using System.Text.Json.Serialization;

namespace OnoStore.Cart.API.Model
{
    public class CartItem
    {
        public CartItem()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Valor { get; set; }
        public string Image { get; set; }

        public Guid CartId { get; set; }

        [JsonIgnore] // avoid object cycle during serialization
        public CustomerCart CustomerCart { get; set; }

        internal void AssociateCart(Guid cartId)
        {
            CartId = cartId;
        }

        internal decimal CalculateValue()
        {
            return Quantity * Valor;
        }

        internal void AddUnits(int units)
        {
            Quantity += units;
        }

        internal void UpdateUnits(int units)
        {
            Quantity = units;
        }

        internal bool IsValid()
        {
            return new ItemCartValidation().Validate(this).IsValid;
        }

        public class ItemCartValidation : AbstractValidator<CartItem>
        {
            public ItemCartValidation()
            {
                RuleFor(c => c.ProductId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Invalid Product Id");

                RuleFor(c => c.Name)
                    .NotEmpty()
                    .WithMessage("Name of the product is missing");

                RuleFor(c => c.Quantity)
                    .GreaterThan(0)
                    .WithMessage(item => $"Minimum quantity of item {item.Name} is 1");

                RuleFor(c => c.Quantity)
                    .LessThanOrEqualTo(CustomerCart.MAX_QUANTIDADE_ITEM)
                    .WithMessage(item => $"Maximum quantity of {item.Name} is {CustomerCart.MAX_QUANTIDADE_ITEM}");

                RuleFor(c => c.Valor)
                    .GreaterThan(0)
                    .WithMessage(item => $"The value of {item.Name} needs to be more than 0");
            }
        }
    }
}