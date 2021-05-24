using ElectronicShopDataAccessLayer.Models;
using ElectronicShopDataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopBusinessLogicLayer.Services
{
    public class CategoryService
    {
        private CategoryRepository _categoryRepository;
        private ProductsRepository _productsRepository;

        public CategoryService (CategoryRepository categoryRepository, ProductsRepository productsRepository)
        {
            _categoryRepository = categoryRepository;
            _productsRepository = productsRepository;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetAsync();
        }

        public async Task<Category> GetCategoryAsync(Category category)
        {
            return await _categoryRepository.GetByIdAsync(category.Id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
        }

        public async Task UpdateCategoryAsync(Category model)
        {

            Category originalC = await _categoryRepository.GetByIdAsync(model.Id);

            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                originalC.Name = model.Name;
            }

            await _categoryRepository.UpdateAsync(originalC);
        }

        public async Task RemoveCategoryAsync(Category category)
        {
            Category cat = await _categoryRepository.GetByIdAsync(category.Id);
            await _productsRepository.RemoveProductsByCategoryIdAsync(cat.Id);
            await _categoryRepository.RemoveAsync(cat);
        }

    }
}
