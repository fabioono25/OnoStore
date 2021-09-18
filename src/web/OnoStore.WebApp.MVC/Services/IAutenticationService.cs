using System.Threading.Tasks;
using OnoStore.WebApp.MVC.Models;

namespace OnoStore.WebApp.MVC.Services
{
    public interface IAutenticationService
    {
        Task<UserResponseLogin> Login(UserLogin userLogin);

        Task<UserResponseLogin> Register(UserRegistry userRegistry);
    }
}