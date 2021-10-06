using Microsoft.Extensions.Options;
using OnoStore.WebApp.MVC.Extensions;
using OnoStore.WebApp.MVC.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using OnoStore.Core.MVC.Models;

namespace OnoStore.WebApp.MVC.Services
{
    public class AuthenticationService : Service, IAutenticationService
    {
        private readonly HttpClient _httpClient;
        //private readonly AppSettings _settings;

        public AuthenticationService(HttpClient httpClient,
                                   IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.AuthenticationUrl);

            _httpClient = httpClient;
            //  _settings = settings.Value;
        }

        public async Task<UserResponseLogin> Login(UserLogin userLogin)
        {
            var loginContent = ObtainContent(userLogin);

            //_settings.AuthenticationUrl
            var response = await _httpClient.PostAsync("/api/identity/authenticate", loginContent);

            // var test = await response.Content.ReadAsStringAsync();

            if (!TransformErrorsResponse(response))
            {
                return new UserResponseLogin
                {
                    ResponseResult = await DeserializeObjectResponse<ResponseResult>(response)
                };
            }

            return await DeserializeObjectResponse<UserResponseLogin>(response);
        }

        public async Task<UserResponseLogin> Register(UserRegistry userRegistry)
        {
            var obtainContent = ObtainContent(userRegistry);

            var response = await _httpClient.PostAsync("/api/identity/register", obtainContent);

            if (!TransformErrorsResponse(response))
            {
                return new UserResponseLogin
                {
                    ResponseResult = await DeserializeObjectResponse<ResponseResult>(response)
                };
            }

            return await DeserializeObjectResponse<UserResponseLogin>(response);
        }
    }
}