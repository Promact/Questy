using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Category;
using Promact.Trappist.Repository.Categories;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private ICategoriesRepository _categoriesRepository { get; set; }
        public CategoriesController(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        #region post Method 
        [HttpPost]
        /// <summary>
        /// Post Method 
        /// Will Add a Catagory Name into table
        ///</summary>
        /// <param name="catagory">Object of  class Category</param>
        /// <returns>Json File</returns>
        public JsonResult CatagoryAdd([FromBody] Category category)
        {
            _categoriesRepository.AddCategory(category);
            return Json(category);
        }
        #endregion

        #region PutMethod
        /// <summary>
        /// Put Method
        /// Will Edit a Existing Category from Category Table
        /// </summary>
        /// <param name="Id">Id si the primary key of Category Model</param>
        /// <param name="catagory">Object of  class Category</param>
        /// <returns>Json File</returns>
        [HttpPut("{id}")]
        public JsonResult CategoryEditAsync(long Id, [FromBody] Category category)
        {
            var promise = _categoriesRepository.GetCatagoryId(Id);
            promise.CategoryName = category.CategoryName;
            _categoriesRepository.CategoryEdit(promise);
            return Json(category);
        }
        #endregion
    }
}
