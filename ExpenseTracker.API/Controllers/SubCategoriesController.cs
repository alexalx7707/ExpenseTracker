using ExpenseTracker.API.DTOs;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;

using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/subcategories")]
    [ApiController]
    public class SubCategoriesController : ControllerBase
    {
        private readonly SubCategoryService _subCategoryService;

        public SubCategoriesController(SubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubCategory>>> GetAllSubCategories()
        {
            return (await _subCategoryService.GetAllSubCategoriesAsync()).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubCategory>> GetSubCategoryById(Guid id)
        {
            var subCategory = await _subCategoryService.GetSubCategoryByIdAsync(id);

            if (subCategory == null)
            {
                return NotFound();
            }

            return Ok(subCategory);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> AddSubCategory(SubCategoryDTO subCategory)
        {
            if (subCategory == null)
            {
                return BadRequest();
            }

            var newSubCategory = new SubCategory
            {
                Name = subCategory.Name,
                Id = subCategory.Id,
                ParentCategoryId = subCategory.ParentCategoryId
            };

            return Ok(await _subCategoryService.AddSubCategoryAsync(newSubCategory));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubCategory(Guid id, SubCategoryDTO subCategory)
        {
            if (subCategory == null || id != subCategory.Id)
            {
                return BadRequest();
            }

            var existingSubCategory = await _subCategoryService.GetSubCategoryByIdAsync(id);
            if (existingSubCategory == null)
            {
                return NotFound();
            }
            
            existingSubCategory.Id = subCategory.Id;
            existingSubCategory.Name = subCategory.Name;
            existingSubCategory.ParentCategoryId = subCategory.ParentCategoryId;
            

            var updated = await _subCategoryService.UpdateSubCategoryAsync(existingSubCategory); //what does this line do?
            //the line above calls the UpdateSubCategoryAsync method from the SubCategoryService class and passes the
            //existingSubCategory object as an argument. The method updates the subcategory in the database and returns
            //the updated subcategory object.

            if (updated == null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubCategory(Guid id)
        {
            var existingSubCategory = await _subCategoryService.GetSubCategoryByIdAsync(id);

            if (existingSubCategory == null)
            {
                return NotFound();
            }

            var deleted = await _subCategoryService.DeleteSubCategoryAsync(id);

            if (!deleted)
            {
                return BadRequest();
            }

            //return NoContent();
            return Ok();
        }
    }
}