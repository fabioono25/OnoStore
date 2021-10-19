using OnoStore.Bff.Purchase.Models;
using OnoStore.Cart.API.Services.gRPC;
using System;
using System.Threading.Tasks;

namespace NSE.Bff.Compras.Services.gRPC
{
    public interface ICarrinhoGrpcService
    {
        Task<CartDto> ObterCarrinho();
    }

    public class CarrinhoGrpcService : ICarrinhoGrpcService
    {
        private readonly CartOrders.CartOrdersClient _carrinhoComprasClient;

        public CarrinhoGrpcService(CartOrders.CartOrdersClient carrinhoComprasClient)
        {
            _carrinhoComprasClient = carrinhoComprasClient;
        }

        public async Task<CartDto> ObterCarrinho()
        {
            var response = await _carrinhoComprasClient.GetCartAsync(new GetCartRequest());
            return MapCarrinhoClienteProtoResponseToDTO(response);
        }

        private static CartDto MapCarrinhoClienteProtoResponseToDTO(CartCustomerResponse carrinhoResponse)
        {
            var carrinhoDTO = new CartDto
            {
                ValorTotal = (decimal)carrinhoResponse.Totalvalue,
                Desconto = (decimal)carrinhoResponse.Desconto,
                VoucherUtilizado = carrinhoResponse.Voucherutilizado
            };

            if (carrinhoResponse.Voucher != null)
            {
                carrinhoDTO.Voucher = new VoucherDTO
                {
                    Codigo = carrinhoResponse.Voucher.Codigo,
                    Percentual = (decimal?)carrinhoResponse.Voucher.Percentual,
                    ValorDesconto = (decimal?)carrinhoResponse.Voucher.Valordesconto,
                    TipoDesconto = carrinhoResponse.Voucher.Tipodesconto
                };
            }

            foreach (var item in carrinhoResponse.Itens)
            {
                carrinhoDTO.Itens.Add(new ProductItemCartDto
                {
                    Name = item.Name,
                    Image = item.Image,
                    ProductId = Guid.Parse(item.Productid),
                    Quantity = item.Quantity,
                    Value = (decimal)item.Value
                });
            }

            return carrinhoDTO;
        }
    }
}