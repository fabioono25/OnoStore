using Microsoft.Extensions.Options;
using OnoStore.WebApp.MVC.Extensions;
using OnoStore.WebApp.MVC.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnoStore.WebApp.MVC.Services
{
    public class CatalogService : Service, ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient,
            IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.CatalogUrl);

            _httpClient = httpClient;
            //  _settings = settings.Value;
        }

        public async Task<PagedViewModel<ProductViewModel>> GetAll(int pageSize, int pageIndex, string query = null)
        {
            var response = await _httpClient.GetAsync($"/catalog/products?ps={pageSize}&page={pageIndex}&q={query}");

            TransformErrorsResponse(response);

            return await DeserializeObjectResponse<PagedViewModel<ProductViewModel>>(response);
        }

        public async Task<ProductViewModel> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalog/products/{id}"); // the user needs to be authenticated

            TransformErrorsResponse(response);

            return await DeserializeObjectResponse<ProductViewModel>(response);
        }
    }
}
