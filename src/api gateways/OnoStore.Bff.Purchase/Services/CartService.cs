using System;
using System.Net.Http;
using Microsoft.Extensions.Options;
using OnoStore.Bff.Purchase.Extensions;

namespace OnoStore.Bff.Purchase.Services
{
    public interface ICarrinhoService
    {
    }

    public class CartService : Service, ICarrinhoService
    {
        private readonly HttpClient _httpClient;

        public CartService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CarrinhoUrl);
        }
    }
}