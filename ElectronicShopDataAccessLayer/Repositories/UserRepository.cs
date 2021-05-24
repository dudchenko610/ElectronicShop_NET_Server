using ElectronicShopDataAccessLayer.Contexts;
using ElectronicShopDataAccessLayer.Core.BaseRepositories;
using ElectronicShopDataAccessLayer.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Exceptions;
using Shared.ViewModels.Pagination;
using Shared.ViewModels.User.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopDataAccessLayer.Repositories
{
    public class UserRepository : GenericRepositorySql<User>
    {

        private UserManager<User> _userManager;

        public UserRepository(DbContextSql context, UserManager<User> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetUserByUserNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.UserName == name);
        }

        public async Task<PagedResponse<List<User>>> GetUsersAsync(PaginationFilter paginationFilter, UserFilter validProductFilter , User meUser)
        {
            List<User> users = await _dbSet.ToListAsync();

            users.Remove(meUser);

            List<User> pagedUsers = users
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize)
                .ToList();


            return PaginationHelper.CreatePagedReponse<User>(pagedUsers, paginationFilter, users.Count);
        }

        public async Task<User> GetAdminUserAsync()
        {
            IList<User> users = await _userManager.GetUsersInRoleAsync(Constants.AuthRoles.ADMIN);
            return users[0];
        }

        public async Task<string> GetUserRoleAsync(string username)
        {
            User user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);

            
            if (user == null)
            {
                throw new ServerException(Constants.Errors.Authentication.LOGIN_INCORRECT);
            }

            IList<string> roles = await _userManager.GetRolesAsync(user);

            return roles[0];
        }

    }
}
