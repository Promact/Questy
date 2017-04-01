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
            if (await _categoryRepository.IsCategoryNameExistsAsync(category))
            {
                ModelState.AddModelError("error", _stringConstants.CategoryNameExistsError);
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
            if (await _categoryRepository.IsCategoryNameExistsAsync(category))
            {
                ModelState.AddModelError("error", _stringConstants.CategoryNameExistsError);
                return BadRequest(ModelState);
            }
            await _categoryRepository.UpdateCategoryAsync(category);
            return Ok(category);
        }
        #endregion
    }
}

