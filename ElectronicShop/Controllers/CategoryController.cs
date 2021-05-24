using ElectronicShopBusinessLogicLayer.Services;
using ElectronicShopDataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicShopPresentationLayer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : Controller
    {

        private CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [Route(Constants.Routes.Category.GET_CATEGORIES)]
        public async Task<IActionResult> GetCategories()
        {
            List<Category> categories = await _categoryService.GetCategoriesAsync();
            return Ok(categories);
        }


        [HttpGet]
        [Route(Constants.Routes.Category.GET_CATEGORY)]
        public async Task<IActionResult> GetCategory([FromQuery] int categoryId)
        {
            Category category = await _categoryService.GetCategoryAsync(new Category { Id = categoryId });
            return Ok(category);
        }


        [Authorize(Roles = Constants.AuthRoles.ADMIN)]
        [Route(Constants.Routes.Category.UPDATE_CATEGORY)]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            await _categoryService.UpdateCategoryAsync(category);
            return Ok();
        }


        [Authorize(Roles = Constants.AuthRoles.ADMIN)]
        [Route(Constants.Routes.Category.ADD_CATEGORY)]
        public async Task<IActionResult> AddCategory(Category category)
        {
            await _categoryService.AddCategoryAsync(category);
            return Ok(category);
        }

        [Authorize(Roles = Constants.AuthRoles.ADMIN)]
        [Route(Constants.Routes.Category.REMOVE_CATEGORY)]
        public async Task<IActionResult> RemoveCategory(Category category)
        {
            await _categoryService.RemoveCategoryAsync(category);
            return Ok();
        }
    }
}