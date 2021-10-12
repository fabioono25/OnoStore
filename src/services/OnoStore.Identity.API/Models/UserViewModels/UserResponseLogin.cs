using System;

namespace OnoStore.Identity.API.Models.UserViewModels
{
    public class UserResponseLogin
    {
        public string AccessToken { get; set; }
        public Guid RefreshToken { get; set; }
        public double ExpiresIn { get; set; }
        public UserToken UsuarioToken { get; set; }
    }
}
