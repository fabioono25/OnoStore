
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OnoStore.WebApp.MVC.Models;

namespace OnoStore.WebApp.MVC.Controllers
{
    public class IdentityController: Controller
    {
        //private readonly IAutenticacaoService _autenticacaoService;

        //public IdentityController(IAutenticacaoService autenticacaoService)
        //{
        //    _autenticacaoService = autenticacaoService;
        //}

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserRegistry usuarioRegistry)
        {
            if (!ModelState.IsValid) return View(usuarioRegistry);

            //var resposta = await _autenticacaoService.Registro(usuarioRegistry);

            //if (ResponsePossuiErros(resposta.ResponseResult)) return View(usuarioRegistry);

            //await RealizarLogin(resposta);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLogin userLogin, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(userLogin);

            //var resposta = await _autenticacaoService.Login(userLogin);

            //if (ResponsePossuiErros(resposta.ResponseResult)) return View(userLogin);

            //await RealizarLogin(resposta);

            //if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");

            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        //private async Task RealizarLogin(UsuarioRespostaLogin resposta)
        //{
        //    var token = ObterTokenFormatado(resposta.AccessToken);

        //    var claims = new List<Claim>();
        //    claims.Add(new Claim("JWT", resposta.AccessToken));
        //    claims.AddRange(token.Claims);

        //    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //    var authProperties = new AuthenticationProperties
        //    {
        //        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
        //        IsPersistent = true
        //    };

        //    await HttpContext.SignInAsync(
        //        CookieAuthenticationDefaults.AuthenticationScheme,
        //        new ClaimsPrincipal(claimsIdentity),
        //        authProperties);
        //}

        //private static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        //{
        //    return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        //}
    }
}
