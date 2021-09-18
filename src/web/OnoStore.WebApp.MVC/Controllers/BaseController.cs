using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnoStore.WebApp.MVC.Models;

namespace OnoStore.WebApp.MVC.Controllers
{
    public class BaseController : Controller
    {
        protected bool ResponseHasErrors(ResponseResult resposta)
        {
            if (resposta != null && resposta.Errors.Messages.Any())
            {
                foreach (var message in resposta.Errors.Messages)
                {
                    ModelState.AddModelError(string.Empty, message);
                }

                return true;
            }

            return false;
        }
    }
}
