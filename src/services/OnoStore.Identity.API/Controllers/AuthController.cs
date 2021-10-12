using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnoStore.Core.Messages.Integration;
using OnoStore.Identity.API.Models.UserViewModels;
using OnoStore.MessageBus;
using OnoStore.WebAPI.Core.Controllers;
using OnoStore.WebAPI.Core.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using NetDevPack.Security.Jwt.Interfaces;
using OnoStore.WebAPI.Core.User;

namespace OnoStore.Identity.API.Controllers
{
    [ApiController]
    [Route("api/identity")]
    public class AuthController : BaseController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        private readonly IAspNetUser _aspNetUser;
        private readonly IJsonWebKeySetService _jwksService;
        private IMessageBus _bus;
        
        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<AppSettings> appSettings, IMessageBus bus, IAspNetUser aspNetUser, IJsonWebKeySetService jwksService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _bus = bus;
            _aspNetUser = aspNetUser;
            _jwksService = jwksService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegistry userRegistry)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = userRegistry.Email,
                Email = userRegistry.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, userRegistry.Password);

            if (result.Succeeded)
            {
                // this is the moment of integration with Customer API (using a Bus)
                var customerResult = await RegisterCustomer(userRegistry);

                if (!customerResult.ValidationResult.IsValid)
                {
                    await _userManager.DeleteAsync(user);
                    return CustomResponse(customerResult.ValidationResult);
                }

                // await _signInManager.SignInAsync(user, false); - it's possible to login now
                //return Ok(await GenerateJwt(userRegistry.Email));
                return CustomResponse(await GenerateJwt(userRegistry.Email));
            }

            foreach (var error in result.Errors)
            {
                AddErrorProcessing(error.Description);
            }

            return CustomResponse();
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult> Login(UserLogin userLogin)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password,
                false, true);

            if (result.Succeeded)
            {
                //return Ok(await GenerateJwt(userLogin.Email));
                return CustomResponse(await GenerateJwt(userLogin.Email));
            }

            if (result.IsLockedOut)
            {
                AddErrorProcessing("User temporarily blocked after many attempts.");
                return CustomResponse();
            }
            AddErrorProcessing("Wrong user or password.");
            return CustomResponse();

            //return BadRequest();
        }

        private async Task<UserResponseLogin> GenerateJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email); // email exists already
            var claims = await _userManager.GetClaimsAsync(user);

            var identityClaims = await ObtainClaimsUser(claims, user);
            var encodedToken = CodifyToken(identityClaims);

            return ObtainResponseToken(encodedToken, user, claims);
        }

        private async Task<ClaimsIdentity> ObtainClaimsUser(ICollection<Claim> claims, IdentityUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }

        private string CodifyToken(ClaimsIdentity identityClaims)
        {
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            //var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            //{
            //    Issuer = _appSettings.Emitter,
            //    Audience = _appSettings.ValidIn,
            //    Subject = identityClaims,
            //    Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //});

            var tokenHandler = new JwtSecurityTokenHandler();

            var currentIssuer =
                $"{_aspNetUser.GetHttpContext().Request.Scheme}://{_aspNetUser.GetHttpContext().Request.Host}";

            var key = _jwksService.GetCurrentSigningCredentials();
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = currentIssuer, // coming from the authentication API 
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = key
            });


            return tokenHandler.WriteToken(token);
        }

        private UserResponseLogin ObtainResponseToken(string encodedToken, IdentityUser user, IEnumerable<Claim> claims)
        {
            return new UserResponseLogin
            {
                AccessToken = encodedToken,
                //ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationHours).TotalSeconds,
                ExpiresIn = TimeSpan.FromHours(1).TotalSeconds,
                UsuarioToken = new UserToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UserClaim { Type = c.Type, Value = c.Value })
                }
            };
        }

        private async Task<ResponseMessage> RegisterCustomer(UserRegistry userRegistry)
        {
            var user = await _userManager.FindByEmailAsync(userRegistry.Email);

            var userRegistered = new UserRegisteredIntegrationEvent(Guid.Parse(user.Id), userRegistry.Name, userRegistry.Email, userRegistry.Cpf);

            try
            {
                //_bus = RabbitHutch.CreateBus("host:localhost:5672");

                return await _bus.RequestAsync<UserRegisteredIntegrationEvent, ResponseMessage>(userRegistered);
            }
            catch
            {
                await _userManager.DeleteAsync(user);
                throw;
            }
        }
        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
