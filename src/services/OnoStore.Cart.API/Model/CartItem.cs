using System;
using System.Text.Json.Serialization;
using FluentValidation;

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

        [JsonIgnore]
        public CustomerCart CustomerCart { get; set; }

        internal void AssociateCart(Guid carrinhoId)
        {
            CartId = carrinhoId;
        }

        internal decimal CalculateValue()
        {
            return Quantity * Valor;
        }

        internal void AddUnits(int unidades)
        {
            Quantity += unidades;
        }

        internal void UpdateUnits(int unidades)
        {
            Quantity = unidades;
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
                    .WithMessage("Id do produto inválido");

                RuleFor(c => c.Name)
                    .NotEmpty()
                    .WithMessage("O nome do produto não foi informado");

                RuleFor(c => c.Quantity)
                    .GreaterThan(0)
                    .WithMessage(item => $"A quantidade miníma para o {item.Name} é 1");

                RuleFor(c => c.Quantity)
                    .LessThanOrEqualTo(CustomerCart.MAX_QUANTIDADE_ITEM)
                    .WithMessage(item => $"A quantidade máxima do {item.Name} é {CustomerCart.MAX_QUANTIDADE_ITEM}");

                RuleFor(c => c.Valor)
                    .GreaterThan(0)
                    .WithMessage(item => $"O valor do {item.Name} precisa ser maior que 0");
            }
        }
    }
}