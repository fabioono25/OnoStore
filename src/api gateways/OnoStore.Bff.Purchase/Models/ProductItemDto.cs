using System;

namespace OnoStore.Bff.Purchase.Models
{
    public class ProductItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string Image { get; set; }
        public int QuantityAvailable { get; set; }
    }
}