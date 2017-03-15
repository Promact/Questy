using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Category;
using System;
using System.Linq;

namespace Promact.Trappist.Repository.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TrappistDbContext _dbContext;

        public CategoryRepository(TrappistDbContext dbContext)
        {
            _dbContext = dbContext;
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
        public Category Getcategory(int key)
        {
            try
            {
                return _dbContext.Category.FirstOrDefault(Check => Check.Id == key);
            }
            catch (Exception)
            {
                return null;
            }
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
    }

}
