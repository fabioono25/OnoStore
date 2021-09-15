using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnoStore.Identity.API.Models.UserViewModels;

namespace OnoStore.Identity.API.Controllers
{
    [Route("api/identity")]
    public class AutoController: Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AutoController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("new-account")]
        public async Task<ActionResult> Register(UserRegistry userRegistry)
        {
            if (!ModelState.IsValid) return BadRequest();//CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = userRegistry.Mail,
                Email = userRegistry.Mail,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, userRegistry.Password);

            if (result.Succeeded)
            {
                // await _signInManager.SignInAsync(user, false); - it's possible to login now
                return Ok();
                //return CustomResponse(await GerarJwt(usuarioRegistro.Email));
            }

            //foreach (var error in result.Errors)
            //{
            //    AdicionarErroProcessamento(error.Description);
            //}

            //return CustomResponse();
            return BadRequest();
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult> Login(UserLogin userLogin)
        {
            if (!ModelState.IsValid) return BadRequest();//CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(userLogin.Mail, userLogin.Password,
                false, true);

            if (result.Succeeded)
            {
                return Ok();
                //return CustomResponse(await GerarJwt(usuarioLogin.Email));
            }

            //if (result.IsLockedOut)
            //{
            //    AdicionarErroProcessamento("Usuário temporariamente bloqueado por tentativas inválidas");
            //    return CustomResponse();
            //}

            //AdicionarErroProcessamento("Usuário ou Senha incorretos");
            //return CustomResponse();

            return BadRequest();
        }
    }
}
