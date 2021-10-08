using NSE.WebApp.MVC.Models;
using System;
using System.Collections.Generic;

namespace OnoStore.WebApp.MVC.Models
{
    public class CartViewModel
    {
        public decimal ValorTotal { get; set; }
        public VoucherViewModel Voucher { get; set; }
        public bool VoucherUtilizado { get; set; }
        public decimal Desconto { get; set; }
        public List<CartProductItemViewModel> Items { get; set; } = new List<CartProductItemViewModel>();
    }

    public class CartProductItemViewModel
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Value { get; set; }
        public string Image { get; set; }
    }
}
