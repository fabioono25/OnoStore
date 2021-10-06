using System;

namespace OnoStore.Bff.Purchase.Models
{
    public class ProductItemCartDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
    }
}