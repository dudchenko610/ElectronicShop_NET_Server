using ElectronicShopBusinessLogicLayer.Providers;
using ElectronicShopDataAccessLayer.Contexts;
using ElectronicShopDataAccessLayer.Models;
using ElectronicShopDataAccessLayer.Repositories;
using ElectronicShopDataAccessLayer.Services;
using Microsoft.AspNetCore.Identity;
using Shared.Constants;
using Shared.Exceptions;
using Shared.ViewModels.Authentication;
using Shared.ViewModels.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopBusinessLogicLayer.Services
{
    public class AuthService
    {

        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private DbContextSql _context;
        private UserRepository _userRepository;
        private UserService _userService;
        private JwtProvider _jwtProvider;


        public AuthService(SignInManager<User> signInManager,
            UserManager<User> userManager,
            DbContextSql context,
            UserRepository userRepository,
            JwtProvider jwtProvider,
            UserService userService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _userService = userService;
        }

        public async Task<string> GetUserRoleAsync(string userName)
        {
            return await _userRepository.GetUserRoleAsync(userName);
        }

        public async Task<TokenModel> LoginAsync(LoginModel model)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(model.PhoneNumber))
            {
                errors.Add(Constants.Errors.Authentication.PHONE_NUMBER_IS_REQUIRED);
            }

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                errors.Add(Constants.Errors.Authentication.PASSWORD_IS_REQUIRED);
            }

            if (errors.Any())
            {
                throw new ServerException(errors);
            }

            User user = await _userManager.FindByNameAsync(model.PhoneNumber);

            if (user is null)
            {
                throw new ServerException(Constants.Errors.Authentication.LOGIN_INCORRECT);
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                throw new ServerException(Constants.Errors.Authentication.INVALID_PASSWORD);
            }

            var claims = await _jwtProvider.GetUserClaimsAsync(user.PhoneNumber);
            string accessToken = _jwtProvider.GenerateAccessToken(claims);
            string refreshToken = _jwtProvider.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            await _userRepository.UpdateAsync(user);

            TokenModel tokens = new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return tokens;

        }

       
        public async Task<TokenModel> RegisterAsync(RegisterModel model)
        {
            User user = await _userService.CreateUserAsync(model);

            var claims = await _jwtProvider.GetUserClaimsAsync(user.PhoneNumber);
            string accessToken = _jwtProvider.GenerateAccessToken(claims);
            string refreshToken = _jwtProvider.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            await _userRepository.UpdateAsync(user);

            TokenModel tokens = new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return tokens;
        }

        public async Task LogoutAsync()
        {
            User user = await _userService.GetMyUserAsync();
            user.RefreshToken = null; // to forget refesh token
            await _userRepository.UpdateAsync(user);
        }

        public async Task<TokenModel> UpdateTokensAsync(TokenModel model)
        {
            if (model is null)
            {
                throw new ServerException(Constants.Errors.Request.INVALID_CLIENT_REQUEST);
            }

            string refreshToken = model.RefreshToken;

            var principal = _jwtProvider.ValidateToken(model.AccessToken);

            string userName = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(userName);

            if (userName is null || !user.RefreshToken.Equals(refreshToken))
            {
                throw new ServerException(Constants.Errors.Authentication.PLEASE_LOGIN);
            }

            string newAccessToken = _jwtProvider.GenerateAccessToken(principal.Claims);
            string newRefreshToken = _jwtProvider.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userRepository.UpdateAsync(user);

            var newTokens = new TokenModel
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return newTokens;
        }
    }
}
