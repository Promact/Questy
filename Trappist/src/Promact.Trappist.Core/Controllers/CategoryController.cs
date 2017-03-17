﻿using Microsoft.AspNetCore.Mvc;
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
        /// Will Add a Catagory Name in Category Model
        ///</summary>
        /// <param name="category">Object of  class Category</param>
        /// <returns>object of the class </returns>
        public IActionResult catagoryAdd([FromBody] Category category)
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
        /// Put Method
        /// Will Edit a Existing Category from Category Model
        /// </summary>
        /// <param name="catagory">Object of  class Category</param>
        /// <returns>object of the class after key found </returns>
        [HttpPut("{id}")]
        public IActionResult CategoryEdit([FromRoute] int id, [FromBody] Category category)
        {
            var categoryName = category.CategoryName;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _categoryRepository.CategoryEdit(id, category);
            return Ok(category);
        }
            #endregion

        #region Check DupliCate Category name
        /// <summary>
        /// Check whether Category Name Exists in Database or not
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns>
        /// true if Name Exists in Database
        /// False if not Exists
        /// </returns>
        [HttpPost("check Duplicate Categoryname")]
        public IActionResult CheckDuplicateCategoryName([FromBody]string categoryName)
        {
            return Ok(_categoryRepository.CheckDuplicateCategoryName(categoryName));
        }
        #endregion
        }

    }
