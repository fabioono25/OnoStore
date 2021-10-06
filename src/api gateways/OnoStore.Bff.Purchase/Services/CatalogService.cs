using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OnoStore.Bff.Purchase.Extensions;
using OnoStore.Bff.Purchase.Models;

namespace OnoStore.Bff.Purchase.Services
{
    public interface ICatalogService
    {
        Task<ProductItemDto> ObterPorId(Guid id);
        Task<IEnumerable<ProductItemDto>> ObterItens(IEnumerable<Guid> ids);
    }

    public class CatalogService : Service, ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl);
        }

        public async Task<ProductItemDto> ObterPorId(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalogo/produtos/{id}");

            TreatErrorsResponse(response);

            return await DeserializeObjectResponse<ProductItemDto>(response);
        }

        public async Task<IEnumerable<ProductItemDto>> ObterItens(IEnumerable<Guid> ids)
        {
            var idsRequest = string.Join(",", ids);

            var response = await _httpClient.GetAsync($"/catalogo/produtos/lista/{idsRequest}/");

            TreatErrorsResponse(response);

            return await DeserializeObjectResponse<IEnumerable<ProductItemDto>>(response);
        }
    }
}