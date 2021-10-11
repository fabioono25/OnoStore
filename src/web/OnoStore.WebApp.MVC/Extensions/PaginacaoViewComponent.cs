using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using OnoStore.WebApp.MVC.Models;

namespace OnoStore.WebApp.MVC.Extensions
{
    public class PaginacaoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IPagedList modeloPaginado)
        {
            return View(modeloPaginado);
        }
    }
}