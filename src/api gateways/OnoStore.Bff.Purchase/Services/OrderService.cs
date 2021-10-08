using Microsoft.Extensions.Options;
using OnoStore.Bff.Purchase.Extensions;
using OnoStore.Bff.Purchase.Models;
using OnoStore.Core.MVC.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnoStore.Bff.Purchase.Services
{
    public interface IPedidoService
    {
        Task<ResponseResult> FinalizarPedido(PedidoDTO pedido);
        Task<PedidoDTO> ObterUltimoPedido();
        Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId();

        Task<VoucherDTO> ObterVoucherPorCodigo(string codigo);
    }

    public class PedidoService : Service, IPedidoService
    {
        private readonly HttpClient _httpClient;

        public PedidoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.PedidoUrl);
        }

        public async Task<ResponseResult> FinalizarPedido(PedidoDTO pedido)
        {
            var pedidoContent = GetContent(pedido);

            var response = await _httpClient.PostAsync("/pedido/", pedidoContent);

            if (!TreatErrorsResponse(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<PedidoDTO> ObterUltimoPedido()
        {
            var response = await _httpClient.GetAsync("/pedido/ultimo/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TreatErrorsResponse(response);

            return await DeserializeObjectResponse<PedidoDTO>(response);
        }

        public async Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId()
        {
            var response = await _httpClient.GetAsync("/pedido/lista-cliente/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TreatErrorsResponse(response);

            return await DeserializeObjectResponse<IEnumerable<PedidoDTO>>(response);
        }

        public async Task<VoucherDTO> ObterVoucherPorCodigo(string codigo)
        {
            var response = await _httpClient.GetAsync($"/voucher/{codigo}/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TreatErrorsResponse(response);

            return await DeserializeObjectResponse<VoucherDTO>(response);
        }
    }
}