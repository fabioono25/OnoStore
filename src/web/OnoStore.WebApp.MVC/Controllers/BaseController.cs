using Microsoft.AspNetCore.Mvc;
using OnoStore.WebApp.MVC.Models;
using System.Linq;

namespace OnoStore.WebApp.MVC.Controllers
{
    public class BaseController : Controller
    {
        protected bool ResponseHasErrors(ResponseResult response)
        {
            if (response != null && response.Errors.Messages.Any())
            {
                foreach (var message in response.Errors.Messages)
                {
                    ModelState.AddModelError(string.Empty, message);
                }

                return true;
            }

            return false;
        }

        protected void AddErrorValidation(string mensagem)
        {
            ModelState.AddModelError(string.Empty, mensagem);
        }

        protected bool IsValidOperation()
        {
            return ModelState.ErrorCount == 0;
        }
    }
}
