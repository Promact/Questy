﻿using Microsoft.AspNetCore.Mvc;
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
            _categoryRepository.AddCategory(category);
            return Ok(category);
        }

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
            var previousCategory = _categoryRepository.GetCategory(Id);
            if (previousCategory == null)
            {
                return NotFound();
            }
            previousCategory.CategoryName = category.CategoryName;
            _categoryRepository.CategoryEdit(previousCategory);
            return Ok(category);
        }
        #endregion
    }
}
