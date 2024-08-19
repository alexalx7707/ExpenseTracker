using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Guid> AddCategoryAsync(Category category);
        Task<Category> GetCategoryByIdAsync(Guid id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(Guid id);
    }
}
