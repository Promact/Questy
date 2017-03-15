using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Category;
using Promact.Trappist.Repository.Categories;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/category")]
    public class CategoryController : Controller
    {
        private ICategoryRepository _categoriesRepository;
        public CategoryController(ICategoryRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>Category List</returns>
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categoryList = _categoriesRepository.GetAllCategories();
            return Ok(categoryList);
        }
        #region post Method 
        [HttpPost]
        /// <summary>
        /// Post Method 
        /// Will Add a Catagory Name in Category Model
        ///</summary>
        /// <param name="category">Object of  class Category</param>
        /// <returns>object of the class </returns>
        public IActionResult CatagoryAdd([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _categoriesRepository.AddCategory(category);
            return Ok(category);
        }
        #endregion

        #region PutMethod
        /// <summary>
        /// Put Method
        /// Will Edit a Existing Category from Category Table
        /// </summary>
        /// <param name="Id">Id is the primary key of Category Model</param>
        /// <param name="catagory">Object of  class Category</param>
        /// <returns>object of the class if key found or else it will return Bad request</returns>
        [HttpPut("{id}")]
        public IActionResult CategoryEdit(int Id, [FromBody] Category category)
        {
            if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            var promise = _categoriesRepository.Getcategory(Id);
            if (promise == null)
                {
                    return BadRequest();
                }
                else
                {
                    promise.CategoryName = category.CategoryName;
                    _categoriesRepository.CategoryEdit(promise);
                    return Ok(category);
                }
                #endregion

            }
        }
    }

