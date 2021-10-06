using System.Collections.Generic;

namespace OnoStore.Bff.Purchase.Models
{
    // must be same properties from CartViewModel
    public class VoucherDTO
    {
        public decimal ValorTotal { get; set; }
        public VoucherDTO Voucher { get; set; }
        public bool VoucherUtilizado { get; set; }
        public decimal Desconto { get; set; }
        public List<ProductItemCartDto> Items { get; set; } = new List<ProductItemCartDto>();
    }
}