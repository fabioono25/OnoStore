using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using OnoStore.Core.MVC.Models;
using OnoStore.WebAPI.Core.User;
using OnoStore.WebApp.MVC.Extensions;
using OnoStore.WebApp.MVC.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnoStore.WebApp.MVC.Services
{
    public class AuthenticationService : Service, IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        //private readonly AppSettings _settings;
        private readonly IAspNetUser _user;
        private readonly Microsoft.AspNetCore.Authentication.IAuthenticationService _authenticationService;

        public AuthenticationService(HttpClient httpClient,
                                   IOptions<AppSettings> settings, IAspNetUser user, Microsoft.AspNetCore.Authentication.IAuthenticationService authenticationService)
        {
            httpClient.BaseAddress = new Uri(settings.Value.AuthenticationUrl);

            _httpClient = httpClient;
            _user = user;
            _authenticationService = authenticationService;
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

        public async Task<UserResponseLogin> Registro(UserRegistry usuarioRegistro)
        {
            var registroContent = ObtainContent(usuarioRegistro);

            var response = await _httpClient.PostAsync("/api/identidade/nova-conta", registroContent);

            if (!TransformErrorsResponse(response))
            {
                return new UserResponseLogin
                {
                    ResponseResult = await DeserializeObjectResponse<ResponseResult>(response)
                };
            }

            return await DeserializeObjectResponse<UserResponseLogin>(response);
        }

        public async Task<UserResponseLogin> UtilizarRefreshToken(string refreshToken)
        {
            var refreshTokenContent = ObtainContent(refreshToken);

            var response = await _httpClient.PostAsync("/api/identidade/refresh-token", refreshTokenContent);

            if (!TransformErrorsResponse(response))
            {
                return new UserResponseLogin
                {
                    ResponseResult = await DeserializeObjectResponse<ResponseResult>(response)
                };
            }

            return await DeserializeObjectResponse<UserResponseLogin>(response);
        }

        public async Task RealizarLogin(UserResponseLogin resposta)
        {
            var token = ObterTokenFormatado(resposta.AccessToken);

            var claims = new List<Claim>();
            claims.Add(new Claim("JWT", resposta.AccessToken));
            claims.Add(new Claim("RefreshToken", resposta.RefreshToken));
            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                IsPersistent = true
            };

            await _authenticationService.SignInAsync(
                _user.GetHttpContext(),
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public async Task Logout()
        {
            await _authenticationService.SignOutAsync(
                _user.GetHttpContext(),
                CookieAuthenticationDefaults.AuthenticationScheme,
                null);
        }

        public static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }

        public bool TokenExpirado()
        {
            var jwt = _user.GetUserToken();
            if (jwt is null) return false;

            var token = ObterTokenFormatado(jwt);
            return token.ValidTo.ToLocalTime() < DateTime.Now;
        }

        public async Task<bool> RefreshTokenValido()
        {
            var resposta = await UtilizarRefreshToken(_user.ObterUserRefreshToken());

            if (resposta.AccessToken != null && resposta.ResponseResult == null)
            {
                await RealizarLogin(resposta);
                return true;
            }

            return false;
        }
    }
}