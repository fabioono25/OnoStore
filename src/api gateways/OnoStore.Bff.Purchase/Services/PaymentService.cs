using System;
using System.Net.Http;
using Microsoft.Extensions.Options;
using OnoStore.Bff.Purchase.Extensions;

namespace OnoStore.Bff.Purchase.Services
{
    public interface IPagamentoService
    {
    }

    public class PaymentService : Service, IPagamentoService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.PagamentoUrl);
        }
    }
}