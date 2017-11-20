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
        #region Private Members
        private readonly ICategoryRepository _categoryRepository;
        private readonly IStringConstants _stringConstants;
        #endregion

        #region Constructor
        public CategoryController(ICategoryRepository categoryRepository, IStringConstants stringConstants)
        {
            _categoryRepository = categoryRepository;
            _stringConstants = stringConstants;
        }
        #endregion

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
        public async Task<IActionResult> AddCategoryAsync([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _categoryRepository.IsCategoryExistAsync(category.CategoryName, category.Id))
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
        public async Task<IActionResult> UpdateCategoryAsync([FromRoute] int id, [FromBody] Category category)
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
            if (await _categoryRepository.IsCategoryExistAsync(category.CategoryName, id))
            {
                ModelState.AddModelError(_stringConstants.ErrorKey, _stringConstants.CategoryNameExistsError);
                return BadRequest(ModelState);
            }
            if(await _categoryRepository.IsCategoryExistInTestAsync(id))
            {
                ModelState.AddModelError(_stringConstants.ErrorKey, _stringConstants.CategoryExistInTestError);
                return BadRequest(ModelState);
            }

            categoryToUpdate.CategoryName = category.CategoryName;
            await _categoryRepository.UpdateCategoryAsync(categoryToUpdate);
            return Ok(categoryToUpdate);
        }

        /// <summary>
        ///  API to delete Category
        ///</summary>
        /// <param name="categoryId">Id to delete Category</param>
        /// <returns>No content(204) response if id found else not found(404) response</returns>
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategoryAsync([FromRoute] int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category categoryToDelete = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (categoryToDelete == null)
            {
                return NotFound();
            }

            if (await _categoryRepository.IsCategoryExistInQuestionAsync(categoryId))
            {
                ModelState.AddModelError(_stringConstants.ErrorKey, _stringConstants.CategoryExistInQuestionError);
                return BadRequest(ModelState);
            }

            await _categoryRepository.DeleteCategoryAsync(categoryToDelete);
            return NoContent();
        }
        #endregion
    }
}