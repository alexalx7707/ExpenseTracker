using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces
{
    public interface ISubCategoryRepository
    {
        Task<Guid> AddSubCategoryAsync(SubCategory subCategory);
        Task<SubCategory> GetSubCategoryByIdAsync(Guid id);
        Task<IEnumerable<SubCategory>> GetAllSubCategoriesAsync();
        Task<SubCategory?> UpdateSubCategoryAsync(SubCategory subCategory);
        Task<bool> DeleteSubCategoryAsync(Guid id);
    }
}
