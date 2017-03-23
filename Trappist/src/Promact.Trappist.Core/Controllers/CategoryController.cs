using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Category;
using Promact.Trappist.Repository.Categories;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>Category List</returns>
        [Route("category")]
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categoryList = _categoryRepository.GetAllCategories();
            return Ok(categoryList);
        }

        [Route("category")]
        [HttpPost]
        /// <summary>
        /// Post Method 
        /// Method to Add Category
        ///</summary>
        /// <param name="category">category object contains category details</param>
        /// <returns>category object contains category details</returns>
        public async Task<IActionResult> CategoryAddAsync([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _categoryRepository.AddCategoryAsync(category);
            return Ok(category);
        }

        /// <summary>
        /// Method to Update Existing Category
        /// </summary>
        /// <param name="id">Take from Route whose Property to be Update</param>
        /// <param name="category">category object contains category details</param>
        /// <returns>Updated category object contains category details</returns>
        [Route("category/{id}")]
        [HttpPut]
        public async Task<IActionResult> CategoryUpdateAsync([FromRoute] int id, [FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var searchCategoryId = await _categoryRepository.SearchForCategoryIdAsync(id);
            if (!searchCategoryId)
            {
                return NotFound();
            }
            await _categoryRepository.CategoryUpdateAsync(id, category);
            return Ok(category);
        }

        /// <summary>
        /// Check whether Category Name Exists in Database or not
        /// </summary>
        /// <param name="categoryName">categoryname to check</param>
        /// <returns>
        /// true if Name Exists in Database
        /// False if not Exists
        /// </returns>
        [Route("category/checkduplicatecategoryname")]
        [HttpPost]
        public async Task<IActionResult> CheckDuplicateCategoryNameAsync([FromBody]string categoryName)
        {
            return Ok(await _categoryRepository.CheckDuplicateCategoryNameAsync(categoryName));
        }
    }
}
