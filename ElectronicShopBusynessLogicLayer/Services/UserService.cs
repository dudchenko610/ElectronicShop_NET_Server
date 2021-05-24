using System;
using System.Threading.Tasks;
using ElectronicShopDataAccessLayer.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

using ElectronicShopDataAccessLayer.Repositories;
using ElectronicShopDataAccessLayer.Services;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using ElectronicShopDataAccessLayer.ViewModels.User;
using Shared.ViewModels.Pagination;
using Shared.ViewModels.User.Filter;
using Shared.ViewModels.Files;
using Shared.ViewModels.Authentication;
using Shared.Constants;
using System.Linq;
using Shared.Exceptions;

namespace ElectronicShopBusinessLogicLayer.Services
{
    public class UserService
    {

        private string _stringConnection;


        private UserRepository _userRepository;
        private FileService _fileService;

        private UserManager<User> _userManager;
        private IHttpContextAccessor _httpContextAccessor;


        public UserService(UserRepository userRepository, FileService fileService, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;

            _fileService = fileService;

            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<User> CreateUserAsync(RegisterModel model)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                errors.Add(Constants.Errors.Authentication.NAME_IS_REQUIRED);
            }

            if (string.IsNullOrWhiteSpace(model.Surname))
            {
                errors.Add(Constants.Errors.Authentication.SURNAME_IS_REQUIRED);
            }

            if (string.IsNullOrWhiteSpace(model.Age))
            {
                errors.Add(Constants.Errors.Authentication.AGE_IS_REQUIRED);
            }

            if (string.IsNullOrWhiteSpace(model.PhoneNumber))
            {
                errors.Add(Constants.Errors.Authentication.PHONE_NUMBER_IS_REQUIRED);
            }

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                errors.Add(Constants.Errors.Authentication.EMAIL_IS_REQUIRED);
            }

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                errors.Add(Constants.Errors.Authentication.PASSWORD_IS_REQUIRED);
            }

            if (errors.Any())
            {
                throw new ServerException(errors);
            }

            var existingUser = await _userRepository.GetUserByUserNameAsync(model.PhoneNumber);

            if (existingUser != null)
            {
                throw new ServerException(Constants.Errors.Authentication.USER_ALREADY_EXISTS);
            }

            User user = new User
            {
                Name = model.Name,
                Surname = model.Surname,
                Age = model.Age,
                PhoneNumber = model.PhoneNumber,
                UserName = model.PhoneNumber,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                throw new ServerException(Constants.Errors.Authentication.USER_WAS_NOT_REGISTERED);
            }

            var addToRole = await _userManager.AddToRoleAsync(user, Constants.AuthRoles.CLIENT);

            if (!addToRole.Succeeded)
            {
                throw new ServerException(Constants.Errors.Authentication.USER_WAS_NOT_ADDED_TO_ROLE);
            }

            return user;
        }

        public async Task<List<User>> GetAllUsersExceptMeAsync()
        {

            List<User> allUsers = (List < User > ) await _userRepository.GetAsync();
            User meUser = await GetMyUserAsync();

            allUsers.Remove(meUser);

            return allUsers;
        }

        public async Task UpdateUserAsync(User user)
        {
            User myUser = await GetMyUserAsync();

            if (!string.IsNullOrEmpty(user.Name))
            {
                myUser.Name = user.Name;
            }

            if (!string.IsNullOrEmpty(user.Surname))
            {
                myUser.Surname = user.Surname;
            }

            if (!string.IsNullOrEmpty(user.Age))
            {
                myUser.Age = user.Age;
            }

            if (!string.IsNullOrEmpty(user.Email))
            {
                myUser.Email = user.Email;
            }



            await _userRepository.UpdateAsync(myUser);
        }

        public async Task<User> GetMyUserAsync()
        {
            return await _userRepository.GetUserByUserNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
        }

        public async Task<IList<string>> GetUserRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<UserRoleModel> GetUserAndRoleAsync()
        {
            User meUser = await GetMyUserAsync();
            IList<string> roles = await GetUserRolesAsync(meUser);

            string myRole = roles[0];

            return new UserRoleModel
            { 
                User = meUser,
                Role = myRole
            };
        }

        public async Task UploadAvatarAsync(FileModel<object> fileModel)
        {
            _fileService.CheckFileModelConsistency(fileModel);

            User myUser = await GetMyUserAsync();
            await _fileService.SaveFileAsync(fileModel.FormFile, "not_auth/profiles/" + myUser.Id + "/avatar", fileModel.FileName);

            
            if (myUser.AvatarFileName != null && myUser.AvatarFileName != "" && myUser.AvatarFileName != fileModel.FileName)
            {
                _fileService.DeleteFile("not_auth/profiles/" + myUser.Id + "/avatar", myUser.AvatarFileName);
            }

            myUser.AvatarFileName = fileModel.FileName;

            await _userRepository.UpdateAsync(myUser);
        }

        public async Task<PagedResponse<List<User>>> GetUsersAsync(PaginationFilter paginationFilter, UserFilter userFilter)
        {
            User meUser = await GetMyUserAsync();

            PaginationFilter validPaginationFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            UserFilter validProductFilter = new UserFilter(userFilter);

            return await _userRepository.GetUsersAsync(validPaginationFilter, validProductFilter, meUser);
        }

        public async Task<User> GetAdminUserAsync()
        {
            return await _userRepository.GetAdminUserAsync();
        }

        public async Task<User> GetUserAsync(string userId)
        {
            return await _userRepository.GetByIdAsync(userId);
        }

    }
}
