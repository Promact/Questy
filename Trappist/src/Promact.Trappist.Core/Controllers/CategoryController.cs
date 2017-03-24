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
        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await _categoryRepository.GetAllCategoriesAsync());
        }

        [HttpPost]
        /// <summary>
        /// API to add Category
        ///</summary>
        /// <param name="category">Category object</param>
        /// <returns>Category object</returns>
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
            #endregion
        }

        /// <summary>
        /// action to delete a category
        ///</summary>
        /// <param name="categoryId">Id of category</param>
        
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> CategoryRemove([FromRoute] int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categoryData = await _categoryRepository.GetCategory(categoryId);
            if (categoryData != null)
            {
                await _categoryRepository.RemoveCategoryAsync(categoryData);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

    }
}