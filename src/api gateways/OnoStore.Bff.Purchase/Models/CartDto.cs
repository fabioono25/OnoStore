using System.Collections.Generic;

namespace OnoStore.Bff.Purchase.Models
{
    public class CartDto
    {
        public decimal ValorTotal { get; set; }
        public VoucherDTO Voucher { get; set; }
        public bool VoucherUtilizado { get; set; }
        public decimal Desconto { get; set; }
        public List<ProductItemCartDto> Itens { get; set; } = new List<ProductItemCartDto>();
    }
}