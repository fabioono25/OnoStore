using System;
using System.Collections.Generic;

namespace OnoStore.WebApp.MVC.Models
{
    public class CartViewModel
    {
        public decimal ValorTotal { get; set; }
        public List<ProductItemViewModel> Items { get; set; } = new List<ProductItemViewModel>();
    }

    public class ProductItemViewModel
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Value { get; set; }
        public string Image { get; set; }
    }
}
