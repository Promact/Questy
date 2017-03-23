using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Category;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        /// <summary>
        /// Method to Add a Category
        /// </summary>
        /// <param name="catagory">category object contains category details</param>
        public async Task AddCategoryAsync(Category category)
        {
            _dbContext.Category.Add(category);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Method to check Whether Id is Exists or not
        /// </summary>
        /// <param name="key">id which have to search</param>
        /// <returns>true if key found else false</returns>
        public async Task<bool> SearchForCategoryIdAsync(int key)
        {
            var category = await _dbContext.Category.FirstOrDefaultAsync(Check => Check.Id == key);
            if (category == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Method to get category by its Id
        /// </summary>
        /// <param name="key">id that will find category</param>
        /// <returns>category object contains category details</returns>
        public async Task<Category> GetCategoryByIdAsync(int key)
        {
            return await _dbContext.Category.FirstOrDefaultAsync(Check => Check.Id == key);
        }

        /// <summary>
        /// Method to Update Category
        /// </summary>
        /// <param name="id">key whose Property will be Updated</param>
        /// <param name="category">category object contains category details</param>
        public async Task CategoryUpdateAsync(int id, Category category)
        {
            var categoryToUpdate = GetCategoryByIdAsync(id);
            if (categoryToUpdate != null)
            {
                categoryToUpdate.Result.CategoryName = category.CategoryName;
                _dbContext.Category.Update(categoryToUpdate.Result);
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Method to Check Same CategoryName Exists or not
        /// </summary>
        /// <param name="categoryName">categoryname will be checked that it is Exists or not</param>
        /// <returns>true if Exists else false</returns>
        public async Task<bool> CheckDuplicateCategoryNameAsync(string categoryName)
        {
            return await _dbContext.Category.AnyAsync(check => check.CategoryName == categoryName);
        }
    }
}