using System;
using System.Net.Http;
using Microsoft.Extensions.Options;
using OnoStore.Bff.Purchase.Extensions;

namespace OnoStore.Bff.Purchase.Services
{
    public interface ICatalogoService
    {
    }

    public class CatalogService : Service, ICatalogoService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl);
        }
    }
}