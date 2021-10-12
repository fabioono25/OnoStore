using OnoStore.WebApp.MVC.Models;
using System.Threading.Tasks;

namespace OnoStore.WebApp.MVC.Services
{
    public interface IAuthenticationService
    {
        Task<UserResponseLogin> Login(UserLogin userLogin);

        Task<UserResponseLogin> Register(UserRegistry userRegistry);

        Task RealizarLogin(UserResponseLogin resposta);
        Task Logout();

        bool TokenExpirado();

        Task<bool> RefreshTokenValido();
    }
}