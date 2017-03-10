using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Category;
using Promact.Trappist.Repository.Categories;
using System.Collections.Generic;

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
        public IActionResult AddCatagoryName([FromBody] Category categoryName)
        {
            _categoriesRepository.CategoryNameAdd(categoryName);
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
        public IActionResult UpdateCatagoryName(long Id, [FromBody] Category categoryName)
        {
            var promise = _categoriesRepository.CatagoryIdFind(Id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            promise.CategoryName = categoryName.CategoryName;
            _categoriesRepository.CategoryRename(promise);
            return Ok();
        }
        #endregion
    }
}
