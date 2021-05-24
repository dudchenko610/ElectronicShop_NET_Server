using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ElectronicShopBusinessLogicLayer.Services;
using ElectronicShopDataAccessLayer.Models;
using static ElectronicShopDataAccessLayer.Repositories.UserRepository;
using Shared.ViewModels.Files;
using Shared.ViewModels.Pagination;
using Shared.ViewModels.User.Filter;
using Shared.Constants;

namespace ElectronicShopPresentationLayer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {

        private UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [Consumes(Constants.Attributes.MULTIPART_FORM_DATA)]
        [Route(Constants.Routes.User.UPLOAD_AVATAR)]
        public async Task<IActionResult> UploadAvatar([FromForm] FileModel<object> file)
        {
            await _userService.UploadAvatarAsync(file);
            return Ok();
        }

        [Authorize]
        [HttpPost(Constants.Routes.User.UPDATE_USER)]
        public async Task<IActionResult> UpdateUser(User user)
        {
            await _userService.UpdateUserAsync(user);
            return Ok();
        }


        [Authorize]
        [HttpGet(Constants.Routes.User.GET_ME_USER)]
        public async Task<IActionResult> GetMeUser()
        {
            var model = await _userService.GetUserAndRoleAsync();
            return Ok(model);
        }

        [Authorize]
        [HttpGet(Constants.Routes.User.GET_ADMIN_USER)]
        public async Task<IActionResult> GetAdminUser()
        {
            User user = await _userService.GetAdminUserAsync();
            return Ok(user);
        }

        [Authorize]
        [HttpGet(Constants.Routes.User.GET_USERS)]
        public async Task<IActionResult> GetUsers([FromQuery] PaginationFilter paginationFilter, [FromQuery] UserFilter userFilter)
        {
            var model = await _userService.GetUsersAsync(paginationFilter, userFilter);
            return Ok(model);
        }

        [Authorize]
        [HttpGet]
        [Route(Constants.Routes.User.GET_USER)]
        public async Task<IActionResult> GetUser([FromQuery] string userId)
        {
            User model = await _userService.GetUserAsync(userId);
            return Ok(model);
        }


    }
}