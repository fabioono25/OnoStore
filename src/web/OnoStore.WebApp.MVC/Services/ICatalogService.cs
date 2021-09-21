using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnoStore.WebApp.MVC.Models;

namespace OnoStore.WebApp.MVC.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<ProductViewModel>> GetAll();
        Task<ProductViewModel> GetById(Guid id);
    }
}
