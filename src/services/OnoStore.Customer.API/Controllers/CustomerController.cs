using Microsoft.AspNetCore.Mvc;
using OnoStore.Core.Mediator;
using OnoStore.Customer.API.Application.Commands;
using OnoStore.Customer.API.Models;
using OnoStore.WebAPI.Core.Controllers;
using OnoStore.WebAPI.Core.User;
using System;
using System.Threading.Tasks;

namespace OnoStore.Customer.API.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAspNetUser _user;

        public CustomerController(IMediatorHandler mediatorHandler, ICustomerRepository customerRepository, IAspNetUser user)
        {
            _mediatorHandler = mediatorHandler;
            _customerRepository = customerRepository;
            _user = user;
        }

        [HttpGet("customers")]
        public async Task<IActionResult> Index()
        {
            var result = await _mediatorHandler.SendCommand(
                new RegisterCustomerCommand(Guid.NewGuid(), "Fabio", "fabio@ono.com", "63001404663"));

            return CustomResponse(result);
        }

        [HttpGet("cliente/endereco")]
        public async Task<IActionResult> ObterEndereco()
        {
            var endereco = await _customerRepository.ObterEnderecoPorId(_user.GetUserId());

            return endereco == null ? NotFound() : CustomResponse(endereco);
        }

        [HttpPost("cliente/endereco")]
        public async Task<IActionResult> AdicionarEndereco(AdicionarEnderecoCommand endereco)
        {
            endereco.ClienteId = _user.GetUserId();
            return CustomResponse(await _mediatorHandler.SendCommand(endereco));
        }
    }
}
