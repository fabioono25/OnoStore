using System;
using System.Net.Http;
using Microsoft.Extensions.Options;
using OnoStore.Bff.Purchase.Extensions;

namespace OnoStore.Bff.Purchase.Services
{
    public interface IPedidoService
    {
    }

    public class OrderService : Service, IPedidoService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.PedidoUrl);
        }
    }
}