using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Promact.Trappist.DomainModel.Models.Category;
using Promact.Trappist.Repository.Categories;
using System.Threading.Tasks;
namespace Promact.Trappist.Core.Controllers
{
    [Route("api/category")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
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
        /// Method to Add Category
        ///</summary>
        /// <param name="category">category object contains category details</param>
        /// <returns>category object contains category details</returns>
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isduplicateCategoryNameExists = await _categoryRepository.CheckDuplicateCategoryNameAsync(category.CategoryName);
            if (isduplicateCategoryNameExists == true)
            {
                ModelState.AddModelError("error", "Category Name Already Exists");
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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isduplicateCategoryNameExists = await _categoryRepository.CheckDuplicateCategoryNameAsync(category.CategoryName);
            if (isduplicateCategoryNameExists == true)
            {
                ModelState.AddModelError("error", "Category Name Already Exists");
                return BadRequest(ModelState);
            }
            var existsCategory = await _categoryRepository.GetCategoryByIdAsync(id);
            if (existsCategory == null)
            {
                return NotFound();
            }
            await _categoryRepository.UpdateCategoryAsync(category);
            return Ok(category);
        }
    }
        }
        #endregion
    }
}
