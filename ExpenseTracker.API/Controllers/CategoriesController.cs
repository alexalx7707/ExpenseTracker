using ExpenseTracker.API.DTOs;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;

using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            return (await _categoryService.GetAllCategoriesAsync()).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> AddCategory(CategoryDTO category)
        {
            if (category == null)
            {
                return BadRequest();
            }

            var newCategory = new Category
            {
                Name = category.Name
            };

            //return await _categoryService.AddCategoryAsync(newCategory);
            return Ok(await _categoryService.AddCategoryAsync(newCategory));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, CategoryDTO category)
        {
            if (category == null || id != category.Id)
            {
                return BadRequest();
            }

            var existingCategory = await _categoryService.GetCategoryByIdAsync(id);

            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.Name = category.Name;

            var updated = await _categoryService.UpdateCategoryAsync(existingCategory);

            if (updated == null)
            {
                return NotFound();
            }

            //return NoContent();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var existingCategory = await _categoryService.GetCategoryByIdAsync(id);

            if (existingCategory == null)
            {
                return NotFound();
            }

            var deleted = await _categoryService.DeleteCategoryAsync(id);

            if (!deleted)
            {
                return BadRequest();
            }

            //return NoContent();
            return Ok();
        }
    }
}
