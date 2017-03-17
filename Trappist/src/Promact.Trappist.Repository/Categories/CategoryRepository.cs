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
            var categoryOrderedByCreatedDateTime = category.OrderBy(g => g.CreatedDateTime).ToList();
            return categoryOrderedByCreatedDateTime;
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

        #region Check Whether Id Exists or not
        /// <summary>
        /// will check id Exists in Category Model or not
        /// </summary>
        /// <param name="key">take value from Route</param>
        /// <returns></returns>
       
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
        /// <returns></returns>
        #region Finding a Id Respective Category
        /// <summary>
        /// Find a Respective Id from Catagory Table
        /// </summary>
        /// <param name="Key"></param>
        /// <Returns>if key foundthen Return respective category from category table or will return Null</Returns>
        public Category GetCategory(int key)
        {
            return _dbContext.Category.FirstOrDefault(Check => Check.Id == key);
        }
        #endregion
        #region Edit A Category Name
        // <summary>
        // Edit a Category from Category Table
        // </summary>
        // <param name="catagory">object of the class Category</param>
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