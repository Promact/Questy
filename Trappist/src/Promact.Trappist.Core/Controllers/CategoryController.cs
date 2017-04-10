using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Category;
using Promact.Trappist.Repository.Categories;
using Promact.Trappist.Utility.Constants;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/category")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IStringConstants _stringConstants;

        public CategoryController(ICategoryRepository categoryRepository, IStringConstants stringConstants)
        {
            _categoryRepository = categoryRepository;
            _stringConstants = stringConstants;
        }

        #region Category API
        /// <summary>
        ///API to get all the Categories
        /// </summary>
        /// <returns>Category List</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            return Ok(await _categoryRepository.GetAllCategoriesAsync());
        }

        /// <summary>
        /// API to add Category
        ///</summary>
        /// <param name="category">Category object</param>
        /// <returns>Category object</returns>
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _categoryRepository.IsCategoryNameExistsAsync(category.CategoryName, category.Id))
            {
                ModelState.AddModelError(_stringConstants.ErrorKey, _stringConstants.CategoryNameExistsError);
                return BadRequest(ModelState);
            }
            await _categoryRepository.AddCategoryAsync(category);
            return Ok(category);
        }

        /// <summary>
        /// API to update Category
        /// </summary>
        /// <param name="id">Id of the Category to be update</param>
        /// <param name="category">Category object</param>
        /// <returns>Category object</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categoryToUpdate = await _categoryRepository.GetCategoryByIdAsync(category.Id);
            if (categoryToUpdate == null)
            {
                return NotFound();
            }
            if (await _categoryRepository.IsCategoryNameExistsAsync(category.CategoryName, id))
            {
                ModelState.AddModelError(_stringConstants.ErrorKey, _stringConstants.CategoryNameExistsError);
                return BadRequest(ModelState);
            }
            categoryToUpdate.CategoryName = category.CategoryName;
            await _categoryRepository.UpdateCategoryAsync(categoryToUpdate);
            return Ok(category);
        }

        /// <summary>
        /// Delete API to remove a Category
        ///</summary>
        /// <param name="categoryId">The id of the Category to delete.</param>
        /// <returns>No content(204) response if id found else not found(404) response</returns>
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Category category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category != null)
            {
                await _categoryRepository.RemoveCategoryAsync(category);
                return NoContent();
            }
            return NotFound();
        }
        #endregion
    }
}