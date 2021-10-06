using Microsoft.Extensions.Options;
using OnoStore.WebApp.MVC.Extensions;
using OnoStore.WebApp.MVC.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using OnoStore.Core.MVC.Models;

namespace OnoStore.WebApp.MVC.Services
{
    public class PurchaseBffService : Service, IPurchaseBffService
    {
        private readonly HttpClient _httpClient;

        public PurchaseBffService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.PurchaseBffUrl);
        }

        public async Task<CartViewModel> GetCart()
        {
            var response = await _httpClient.GetAsync("/purchase/cart/");

            TransformErrorsResponse(response);

            return await DeserializeObjectResponse<CartViewModel>(response);
        }

        public async Task<int> GetQuantityCart()
        {
            var response = await _httpClient.GetAsync("/purchase/cart/quantity/");

            TransformErrorsResponse(response);

            return await DeserializeObjectResponse<int>(response);
        }

        public async Task<ResponseResult> AddItemCart(CartProductItemViewModel cartProduct)
        {
            var itemContent = ObtainContent(cartProduct);

            var response = await _httpClient.PostAsync("/purchase/cart/items/", itemContent);

            if (!TransformErrorsResponse(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> UpdateItemCart(Guid productId, CartProductItemViewModel cartProduct)
        {
            var itemContent = ObtainContent(cartProduct);

            var response = await _httpClient.PutAsync($"/purchase/cart/items/{cartProduct.ProductId}", itemContent);

            if (!TransformErrorsResponse(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> RemoveItemCart(Guid productId)
        {
            var response = await _httpClient.DeleteAsync($"/purchase/cart/items/{productId}");

            if (!TransformErrorsResponse(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
