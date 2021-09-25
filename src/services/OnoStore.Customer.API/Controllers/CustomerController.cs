using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnoStore.Core.Mediator;
using OnoStore.Customer.API.Application.Commands;
using OnoStore.WebAPI.Core.Controllers;

namespace OnoStore.Customer.API.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public CustomerController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet("customers")]
        public async Task<IActionResult> Index()
        {
            var result = await _mediatorHandler.SendCommand(
                new RegisterCustomerCommand(Guid.NewGuid(), "Fabio", "fabio@ono.com", "63001404663"));
           
            return CustomResponse(result);
        }
    }
}
