using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Category;
using Promact.Trappist.Repository.Categories;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Promact.Trappist.Core.Controllers
{
    [Produces("application/json")]
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
        public async Task<JsonResult> CatagoryAddAsync([FromBody] Category category)
        {
            await _categoriesRepository.AddCategoryAsync(category);
            return Json(category);
        }
        #endregion

        #region PutMethod
        /// <summary>
        /// Put Method
        /// Will Edit a Existing CategoryNAme from Category Table
        /// </summary>
        /// <param name="Id">Id si the primary key of Category Model</param>
        /// <param name="catagory">Object of  class Category</param>
        /// <returns>Json File</returns>
        [HttpPut("{id}")]
        public async Task<JsonResult> CategoryEditAsync(long Id, [FromBody] Category category)
        {
            var promise = _categoriesRepository.GetCatagoryId(Id);
            promise.CategoryName = category.CategoryName;
            await _categoriesRepository.CategoryEditAsync(promise);
            return Json(category);
        }
        #endregion
    }
}
