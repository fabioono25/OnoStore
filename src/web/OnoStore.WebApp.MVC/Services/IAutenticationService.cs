using OnoStore.WebApp.MVC.Models;
using System.Threading.Tasks;

namespace OnoStore.WebApp.MVC.Services
{
    public interface IAutenticationService
    {
        Task<UserResponseLogin> Login(UserLogin userLogin);

        Task<UserResponseLogin> Register(UserRegistry userRegistry);
    }
}