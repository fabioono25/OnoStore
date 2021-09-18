using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnoStore.WebApp.MVC.Models;
using System.Diagnostics;

namespace OnoStore.WebApp.MVC.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("error/{id:length(3,3)}")]
        public IActionResult Error(int id)
        {
            var modelErro = new ErrorViewModel();

            if (id == 500)
            {
                modelErro.Message = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Title = "Ocorreu um erro!";
                modelErro.ErrorCode = id;
            }
            else if (id == 404)
            {
                modelErro.Message =
                    "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
                modelErro.Title = "Ops! Página não encontrada.";
                modelErro.ErrorCode = id;
            }
            else if (id == 403) // I know who you are but you have no access - 401: I don't know who you are, you must login
            {
                modelErro.Message = "Você não tem permissão para fazer isto.";
                modelErro.Title = "Acesso Negado";
                modelErro.ErrorCode = id;
            }
            else
            {
                return StatusCode(404);
            }

            return View("Error", modelErro);
        }
    }
}
