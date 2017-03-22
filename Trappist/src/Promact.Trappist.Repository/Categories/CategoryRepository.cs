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
        public void CategoryEdit(Category category)
        {
            _dbContext.Category.Update(category);
            _dbContext.SaveChanges();
        }
        #endregion
        /// <summary>
        /// delete a Category from Category model
        /// </summary>
        /// <param name="category"> object of category model</param>
        public void RemoveCategoryToDatabase(Category category)
        {
            _dbContext.Category.Remove(category);
            _dbContext.SaveChanges();            
        }
    }
}