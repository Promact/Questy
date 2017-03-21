using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Category;
using Promact.Trappist.Repository.Categories;

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
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>Category List</returns>
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categoryList = _categoryRepository.GetAllCategories();
            return Ok(categoryList);
        }

        #region post Method 
        [HttpPost]
        /// <summary>
        /// Post Method 
        /// Method to add category
        ///</summary>
        /// <param name="category">category object contains category details</param>
        /// <returns>category object contains category details</returns>
        public IActionResult CatagoryAdd([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _categoryRepository.AddCategory(category);
            return Ok(category);
        }
        #endregion

        #region PutMethod
        /// <summary>
        /// Method to Update Existing Category
        /// </summary>
        /// <param name="id">Take from Route whose Property to be Update</param>
        /// <param name="category">category object contains category details</param>
        /// <returns>Updated category object contains category details</returns>
        [HttpPut("{id}")]
        public IActionResult CategoryEdit([FromRoute] int id, [FromBody] Category category)
        {
            var searchById = _categoryRepository.SearchForCategoryId(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (searchById == false)
            {
                return NotFound();
            }
            _categoryRepository.CategoryUpdate(id, category);
            return Ok(category);
        }
        #endregion

        #region Check DupliCate Category name
        /// <summary>
        /// Check whether Category Name Exists in Database or not
        /// </summary>
        /// <param name="categoryName">categoryname to check</param>
        /// <returns>
        /// true if Name Exists in Database
        /// False if not Exists
        /// </returns>
        [HttpPost("checkduplicatecategoryname")]
        public IActionResult CheckDuplicateCategoryName([FromBody]string categoryName)
        {
            return Ok(_categoryRepository.CheckDuplicateCategoryName(categoryName));
        }
        #endregion
    }

}
