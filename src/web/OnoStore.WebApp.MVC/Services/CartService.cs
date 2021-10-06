using Microsoft.Extensions.Options;
using OnoStore.WebApp.MVC.Extensions;
using OnoStore.WebApp.MVC.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using OnoStore.Core.MVC.Models;

namespace OnoStore.WebApp.MVC.Services
{
    public class CartService : Service, ICartService
    {
        private readonly HttpClient _httpClient;

        public CartService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CartUrl);
        }

        public async Task<CartViewModel> GetCart()
        {
            var response = await _httpClient.GetAsync("/cart/");

            TransformErrorsResponse(response);

            return await DeserializeObjectResponse<CartViewModel>(response);
        }

        public async Task<ResponseResult> AddItemCart(ProductItemViewModel product)
        {
            var itemContent = ObtainContent(product);

            var response = await _httpClient.PostAsync("/cart/", itemContent);

            if (!TransformErrorsResponse(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> UpdateItemCart(Guid productId, ProductItemViewModel product)
        {
            var itemContent = ObtainContent(product);

            var response = await _httpClient.PutAsync($"/cart/{product.ProductId}", itemContent);

            if (!TransformErrorsResponse(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> RemoveItemCart(Guid productId)
        {
            var response = await _httpClient.DeleteAsync($"/cart/{productId}");

            if (!TransformErrorsResponse(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
