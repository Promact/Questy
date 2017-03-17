using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Promact.Trappist.Repository.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TrappistDbContext _dbContext;

        public CategoryRepository(TrappistDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Get all the names of Categories
        /// </summary>
        /// <returns>Categories list</returns>
        public IEnumerable<Category> GetAllCategories()
        {
            var category = _dbContext.Category.ToList();
            return (category);
        }

        #region Adding a CategoryName
        /// <summary>
        /// Adding a Category in Category model
        /// </summary>
        /// <param name="catagory">Object of class Category</param>
        public void AddCategory(Category category)
        {
            _dbContext.Category.Add(category);
            _dbContext.SaveChanges();
        }
        #endregion

        #region Edit A Category Name
        // <summary>
        // Edit a Category from Category Table
        // </summary>
        // <param name="catagory">object of the class Category</param>
        public void CategoryEdit(int id, Category category)
        {
            var categoryToUpdate = _dbContext.Category.FirstOrDefault(check => check.Id == id);
            categoryToUpdate.CategoryName = category.CategoryName;
            _dbContext.Category.Update(categoryToUpdate);
            _dbContext.SaveChanges();
        }
        #endregion

        #region Check Duplicate Category Name Exists or not
        /// <summary>
        /// check whether same Category Name Exists Or not
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns>true if Exists else False</returns>
        public bool CheckDuplicateCategoryName(string categoryName)
        {
            var isCategoryNameExist = _dbContext.Category.Any(check => check.CategoryName == categoryName);
            return isCategoryNameExist;
        }
        #endregion
        #region MyRegion
        public Category GetCategory(int key)
        {
            return _dbContext.Category.FirstOrDefault(Check => Check.Id == key);
        }

        public void CategoryEdit(Category category)
        {
            var categoryToUpdate = _dbContext.Category.FirstOrDefault(check => check.Id == id);
            categoryToUpdate.CategoryName = category.CategoryName;
            _dbContext.Category.Update(categoryToUpdate);
            _dbContext.SaveChanges();
        }
        #endregion

        #region Check Duplicate Category Name Exists or not
        /// <summary>
        /// check whether same Category Name Exists Or not
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns>true if Exists else False</returns>
        public bool CheckDuplicateCategoryName(string categoryName)
        {
            var isCategoryNameExist = _dbContext.Category.Any(check => check.CategoryName == categoryName);
            return isCategoryNameExist;
        }
        #endregion
    }
}
