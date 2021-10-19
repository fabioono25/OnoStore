using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnoStore.Cart.API.Data;
using OnoStore.Cart.API.Model;
using OnoStore.Cart.API.Services.gRPC;
using OnoStore.WebAPI.Core.User;
using System.Threading.Tasks;

namespace NSE.Carrinho.API.Services.gRPC
{
    [Authorize]
    public class CarrinhoGrpcService : CartOrders.CartOrdersBase
    {
        private readonly ILogger<CarrinhoGrpcService> _logger;

        private readonly IAspNetUser _user;
        private readonly CartContext _context;

        public CarrinhoGrpcService(
            ILogger<CarrinhoGrpcService> logger,
            IAspNetUser user,
            CartContext context)
        {
            _logger = logger;
            _user = user;
            _context = context;
        }

        public override async Task<CartCustomerResponse> GetCart(GetCartRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Chamando ObterCarrinho");

            var carrinho = await ObterCarrinhoCliente() ?? new CustomerCart();

            return MapCarrinhoClienteToProtoResponse(carrinho);
        }

        private async Task<CustomerCart> ObterCarrinhoCliente()
        {
            return await _context.CustomerCart
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CustomerId == _user.GetUserId());
        }

        private static CartCustomerResponse MapCarrinhoClienteToProtoResponse(CustomerCart carrinho)
        {
            var carrinhoProto = new CartCustomerResponse
            {
                Id = carrinho.Id.ToString(),
                Customerid = carrinho.CustomerId.ToString(),
                Totalvalue = (double)carrinho.TotalValue,
                Desconto = (double)carrinho.Desconto,
                Voucherutilizado = carrinho.VoucherUtilizado,
            };

            if (carrinho.Voucher != null)
            {
                carrinhoProto.Voucher = new VoucherResponse
                {
                    Codigo = carrinho.Voucher.Codigo,
                    Percentual = (double?)carrinho.Voucher.Percentual ?? 0,
                    Valordesconto = (double?)carrinho.Voucher.ValorDesconto ?? 0,
                    Tipodesconto = (int)carrinho.Voucher.TipoDesconto
                };
            }

            foreach (var item in carrinho.Items)
            {
                carrinhoProto.Itens.Add(new CartItemResponse
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Image = item.Image,
                    Productid = item.ProductId.ToString(),
                    Quantity = item.Quantity,
                    Value = (double)item.Valor
                });
            }

            return carrinhoProto;
        }
    }
}