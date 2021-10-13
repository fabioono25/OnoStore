using Microsoft.AspNetCore.Mvc;
using OnoStore.WebApp.MVC.Models;
using OnoStore.WebApp.MVC.Services;
using System.Threading.Tasks;

namespace OnoStore.WebApp.MVC.Controllers
{
    public class IdentityController : BaseController
    {
        //private readonly IAuthenticationService _autenticationService;

        //public IdentityController(IAuthenticationService autenticationService)
        //{
        //    _autenticationService = autenticationService;
        //}

        //[HttpGet]
        //[Route("register")]
        //public IActionResult Register()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[Route("register")]
        //public async Task<IActionResult> Register(UserRegistry usuarioRegistry)
        //{
        //    if (!ModelState.IsValid) return View(usuarioRegistry);

        //    var response = await _autenticationService.Register(usuarioRegistry);

        //    if (ResponseHasErrors(response.ResponseResult)) return View(usuarioRegistry);

        //    await ExecuteLogin(response);

        //    return RedirectToAction("Index", "Home");
        //}

        //[HttpGet]
        //[Route("login")]
        //public IActionResult Login(string returnUrl = null)
        //{
        //    ViewData["ReturnUrl"] = returnUrl;
        //    return View();
        //}

        //[HttpPost]
        //[Route("login")]
        //public async Task<IActionResult> Login(UserLogin userLogin, string returnUrl = null)
        //{
        //    ViewData["ReturnUrl"] = returnUrl;
        //    if (!ModelState.IsValid) return View(userLogin);

        //    var response = await _autenticationService.Login(userLogin);

        //    if (ResponseHasErrors(response.ResponseResult)) return View(userLogin);

        //    await ExecuteLogin(response);

        //    if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");

        //    return LocalRedirect(returnUrl);
        //}

        //[HttpGet]
        //[Route("logout")]
        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    return RedirectToAction("Index", "Home");
        //}

        //private async Task ExecuteLogin(UserResponseLogin resposta)
        //{
        //    var token = ObtainFormattedToken(resposta.AccessToken);

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

        //private static JwtSecurityToken ObtainFormattedToken(string jwtToken)
        //{
        //    return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        //}
        private readonly IAuthenticationService _autenticacaoService;

        public IdentityController(
            IAuthenticationService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpGet]
        [Route("nova-conta")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("nova-conta")]
        public async Task<IActionResult> Register(UserRegistry usuarioRegistro)
        {
            if (!ModelState.IsValid) return View(usuarioRegistro);

            var resposta = await _autenticacaoService.Register(usuarioRegistro);

            if (ResponseHasErrors(resposta.ResponseResult)) return View(usuarioRegistro);

            await _autenticacaoService.RealizarLogin(resposta);

            return RedirectToAction("Index", "Catalog");
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
        public async Task<IActionResult> Login(UserLogin usuarioLogin, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(usuarioLogin);

            var resposta = await _autenticacaoService.Login(usuarioLogin);

            if (ResponseHasErrors(resposta.ResponseResult)) return View(usuarioLogin);

            await _autenticacaoService.RealizarLogin(resposta);

            if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Catalog");

            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        [Route("sair")]
        public async Task<IActionResult> Logout()
        {
            await _autenticacaoService.Logout();
            return RedirectToAction("Index", "Catalog");
        }
    }
}
