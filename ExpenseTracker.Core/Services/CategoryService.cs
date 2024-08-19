using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Guid> AddCategoryAsync(Category category)
        {
            return await _categoryRepository.AddCategoryAsync(category);
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid categoryId)
        {
            return await _categoryRepository.GetCategoryByIdAsync(categoryId);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllCategoriesAsync();
        }

        public async Task<Category?> UpdateCategoryAsync(Category category)
        {
            return await _categoryRepository.UpdateCategoryAsync(category);
        }

        public async Task<bool> DeleteCategoryAsync(Guid categoryId)
        {
            return await _categoryRepository.DeleteCategoryAsync(categoryId);
        }
    }
}
