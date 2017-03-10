using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Category;
using Promact.Trappist.Repository.Categories;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        public ICategoriesRepository _categoriesRepository { get; set; }
        public CategoriesController(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        #region post Method 
        [HttpPost]
        /// <summary>
        /// Post Method 
        /// Will Add a Catagory Name into table
        /// /// </summary>
        /// <param name="catagoryname"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddIntoCategoryAsync([FromBody] Category category)
        {
            await _categoriesRepository.AddCategoryAsync(category);
            return Ok();
        }
        #endregion

        #region PutMethod
        /// <summary>
        /// Put Method
        /// Will Edit a Existing CategoryNAme from Category Table
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="catagoryname"></param>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditFromCategoryAsync(long Id, [FromBody] Category category)
        {
            var promise = _categoriesRepository.SearchCatagoryId(Id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            promise.CategoryName = category.CategoryName;
            await _categoriesRepository.CategoryEditAsync(promise);
            return Ok();
        }
        #endregion
    }
}
