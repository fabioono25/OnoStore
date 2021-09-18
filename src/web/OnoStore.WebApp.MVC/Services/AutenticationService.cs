using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OnoStore.WebApp.MVC.Extensions;
using OnoStore.WebApp.MVC.Extensions;
using OnoStore.WebApp.MVC.Models;

namespace OnoStore.WebApp.MVC.Services
{
    public class AutenticationService : Service, IAutenticationService
    {
        private readonly HttpClient _httpClient;

        public AutenticationService(HttpClient httpClient,
                                   IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.AuthenticationUrl);

            _httpClient = httpClient;
        }

        public async Task<UserResponseLogin> Login(UserLogin userLogin)
        {
            var loginContent = ObtainContent(userLogin);

            var response = await _httpClient.PostAsync("/api/identity/authenticate", loginContent);

            var test = await response.Content.ReadAsStringAsync();

            if (!TreatErrorsResponse(response))
            {
                return new UserResponseLogin
                {
                    //ResponseResult = await DeserializeObjectResponse<ResponseResult>(response)
                };
            }

            return await DeserializeObjectResponse<UserResponseLogin>(response);
        }

        public async Task<UserResponseLogin> Register(UserRegistry userRegistry)
        {
            var registroContent = ObtainContent(userRegistry);

            var response = await _httpClient.PostAsync("/api/identity/register", registroContent);

            if (!TreatErrorsResponse(response))
            {
                return new UserResponseLogin
                {
                    //ResponseResult = await DeserializeObjectResponse<ResponseResult>(response)
                };
            }

            return await DeserializeObjectResponse<UserResponseLogin>(response);
        }
    }
}