using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ElectronicShop.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


using ElectronicShopBusinessLogicLayer.Services;
using Shared.ViewModels.Authentication;
using Shared.ViewModels.Jwt;
using Shared.Constants;

namespace ElectronicShop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private AuthService _authService;
        
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost(Constants.Routes.Auth.LOGIN)]
        public async Task<IActionResult> Login(LoginModel model)
        {
            TokenModel tokens = await _authService.LoginAsync(model);
            return Ok(tokens);
        }


        // REGISTERING
        [HttpPost(Constants.Routes.Auth.REGISTER)]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            TokenModel tokens = await _authService.RegisterAsync(model);
            return Ok(tokens);
        }

        [HttpPost]
        [Authorize]
        [Route(Constants.Routes.Auth.LOGOUT)]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok();
        }

        [HttpPost]
        [Route(Constants.Routes.Auth.UPDATE_TOKENS)]
        public async Task<IActionResult> UpdateTokensAsync(TokenModel model)
        {
            TokenModel newTokens = await _authService.UpdateTokensAsync(model);
            return Ok(newTokens);
        }
    }
}