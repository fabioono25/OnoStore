using OnoStore.WebApp.MVC.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnoStore.WebApp.MVC.Services
{
    public interface ICatalogService
    {
        //Task<IEnumerable<ProductViewModel>> GetAll();
        Task<PagedViewModel<ProductViewModel>> GetAll(int pageSize, int pageIndex, string query = null);

        Task<ProductViewModel> GetById(Guid id);
    }
}
