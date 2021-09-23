using System;

namespace OnoStore.WebApp.MVC.Models
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public decimal Value { get; set; }
        public DateTime DataInsert { get; set; }
        public string Image { get; set; }
        public int QuantityInventory { get; set; }
    }
}
