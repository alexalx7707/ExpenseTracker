using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services
{
    public class SubCategoryService
    {
        private readonly ISubCategoryRepository _subCategoryRepository;

        public SubCategoryService(ISubCategoryRepository subCategoryRepository)
        {
            _subCategoryRepository = subCategoryRepository;
        }

        public async Task<Guid> AddSubCategoryAsync(SubCategory subCategory)
        {
            return await _subCategoryRepository.AddSubCategoryAsync(subCategory);
        }

        public async Task<SubCategory?> GetSubCategoryByIdAsync(Guid subCategoryId)
        {
            return await _subCategoryRepository.GetSubCategoryByIdAsync(subCategoryId);
        }

        public async Task<IEnumerable<SubCategory>> GetAllSubCategoriesAsync()
        {
            return await _subCategoryRepository.GetAllSubCategoriesAsync();
        }

        public async Task<SubCategory?> UpdateSubCategoryAsync(SubCategory subCategory)
        {
            return await _subCategoryRepository.UpdateSubCategoryAsync(subCategory);
        }

        public async Task<bool> DeleteSubCategoryAsync(Guid subCategoryId)
        {
            return await _subCategoryRepository.DeleteSubCategoryAsync(subCategoryId);
        }
    }
}
