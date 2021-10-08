using Microsoft.Extensions.Options;
using OnoStore.Bff.Purchase.Extensions;
using OnoStore.Bff.Purchase.Models;
using OnoStore.Core.MVC.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnoStore.Bff.Purchase.Services
{
    public interface ICartService
    {
        Task<CartDto> ObterCarrinho();
        Task<ResponseResult> AdicionarItemCarrinho(ProductItemCartDto produto);
        Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ProductItemCartDto carrinho);
        Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
        Task<ResponseResult> AplicarVoucherCarrinho(VoucherDTO voucher);
    }

    public class CartService : Service, ICartService
    {
        private readonly HttpClient _httpClient;

        public CartService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CarrinhoUrl);
        }

        public async Task<CartDto> ObterCarrinho()
        {
            var response = await _httpClient.GetAsync("/carrinho/");

            TreatErrorsResponse(response);

            return await DeserializeObjectResponse<CartDto>(response);
        }

        public async Task<ResponseResult> AdicionarItemCarrinho(ProductItemCartDto produto)
        {
            var itemContent = GetContent(produto);

            var response = await _httpClient.PostAsync("/carrinho/", itemContent);

            if (!TreatErrorsResponse(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ProductItemCartDto carrinho)
        {
            var itemContent = GetContent(carrinho);

            var response = await _httpClient.PutAsync($"/carrinho/{carrinho.ProductId}", itemContent);

            if (!TreatErrorsResponse(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
        {
            var response = await _httpClient.DeleteAsync($"/carrinho/{produtoId}");

            if (!TreatErrorsResponse(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> AplicarVoucherCarrinho(VoucherDTO voucher)
        {
            var itemContent = GetContent(voucher);

            var response = await _httpClient.PostAsync("/carrinho/aplicar-voucher/", itemContent);

            if (!TreatErrorsResponse(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}