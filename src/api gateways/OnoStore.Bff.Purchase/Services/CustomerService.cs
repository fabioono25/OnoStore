using Microsoft.Extensions.Options;
using OnoStore.Bff.Purchase.Extensions;
using OnoStore.Bff.Purchase.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnoStore.Bff.Purchase.Services
{
    public interface ICustomerService
    {
        Task<EnderecoDTO> ObterEndereco();
    }

    public class CustomerService : Service, ICustomerService
    {
        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CustomerUrl);
        }

        public async Task<EnderecoDTO> ObterEndereco()
        {
            var response = await _httpClient.GetAsync("/cliente/endereco/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TreatErrorsResponse(response);

            return await DeserializeObjectResponse<EnderecoDTO>(response);
        }
    }
}