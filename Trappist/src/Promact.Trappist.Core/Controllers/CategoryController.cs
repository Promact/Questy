using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Category;
using Promact.Trappist.Repository.Category;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/Category")]
    public class CategoryController : Controller
    {
        private ICategoryRepository _categoriesRepository;
        public CategoryController(ICategoryRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        #region post Method 
        [HttpPost]
        /// <summary>
        /// Post Method 
        /// Will Add a Catagory Name into table
        ///</summary>
        /// <param name="category">Object of  class Category</param>
        /// <returns>object of the class </returns>
        public IActionResult CatagoryAdd([FromBody] Category category)
        {
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
        /// <returns>object of the class</returns>
        [HttpPut("{id}")]
        public IActionResult CategoryEdit(int Id,[FromBody] Category category)
        {
            var promise = _categoriesRepository.GetId(Id);
            promise.CategoryName = category.CategoryName;
            _categoriesRepository.CategoryEdit(promise);
            return Ok(category);
        }
        #endregion
    }
}
