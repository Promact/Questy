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

        #region Add Category
        /// <summary>
        /// Method to add a Category
        /// </summary>
        /// <param name="catagory">category object contains category details</param>
        public void AddCategory(Category category)
        {
            _dbContext.Category.Add(category);
            _dbContext.SaveChanges();
        }
        #endregion

        #region Check Whether Id Exists or not
        /// <summary>
        /// will check id Exists in Category Model or not
        /// </summary>
        /// <param name="key">take value from Route</param>
        /// <returns>true if key found else false</returns>
        public bool SearchForCategoryId(int key)
        {

            var categoryFind = _dbContext.Category.FirstOrDefault(Check => Check.Id == key);
            if (categoryFind == null)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Find Category of respective Id
        /// <summary>
        /// Find category of respective id
        /// </summary>
        /// <param name="key">id that will find category</param>
        /// <returns>category object contains category details</returns>
        public Category GetCategory(int key)
        {
            return _dbContext.Category.FirstOrDefault(Check => Check.Id == key);
        }
        #endregion

        #region Update  Category
        /// <summary>
        /// Method to Update Category
        /// </summary>
        /// <param name="id">key whose value will be Updated</param>
        /// <param name="category">category object contains category details</param>
        public void CategoryUpdate(int id, Category category)
        {
            var categoryToUpdate = GetCategory(id);
            categoryToUpdate.CategoryName = category.CategoryName;
            _dbContext.Category.Update(categoryToUpdate);
            _dbContext.SaveChanges();
        }
        #endregion

        #region Check Duplicate Category Name Exists or not
        /// <summary>
        /// Method to Check Same CategoryName Exists or not
        /// </summary>
        /// <param name="categoryName">CategoryName</param>
        /// <returns>true if Exists else False</returns>
        public bool CheckDuplicateCategoryName(string categoryName)
        {
            var isCategoryNameExist = _dbContext.Category.Any(check => check.CategoryName == categoryName);
            return isCategoryNameExist;
        }
        #endregion
    }
}
