using OnoStore.Core.MVC.Models;

namespace OnoStore.WebApp.MVC.Models
{
    public class UserResponseLogin
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public double ExpiresIn { get; set; }
        public UserToken UserToken { get; set; }
        public ResponseResult ResponseResult { get; set; }
    }
}
