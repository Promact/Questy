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
        /// Method to get all the Categories
        /// </summary>
        /// <returns>Categories list</returns>
        /// The function name ends with Async
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return (await _dbContext.Category.OrderByDescending(g => g.CreatedDateTime).ToListAsync());
        }

        /// <summary>
        /// Method to add Category
        /// </summary>
        /// <param name="category">category object contains Category details</param>
        /// <returns>Task</returns>
        public async Task AddCategoryAsync(Category category)
        {
            await _dbContext.Category.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Method to get Category by id
        /// </summary>
        /// <param name="key">Id which will get Category</param>
        /// <returns>Task</returns>
        public async Task<Category> GetCategoryByIdAsync(int key)
        {
            return await _dbContext.Category.FindAsync(key);
        }

        /// <summary>
        /// Method to update Category
        /// </summary>
        /// <param name="category">category object contains Category details</param>
        /// <returns>Task</returns>
        public async Task UpdateCategoryAsync(Category category)
        {
            var categoryToUpdate = await GetCategoryByIdAsync(category.Id);
            categoryToUpdate.CategoryName = category.CategoryName;
            _dbContext.Category.Update(categoryToUpdate);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Method to check CategoryName exists or not
        /// </summary>
        /// <param name="category">category object contains Category details</param>
        /// <returns>True if exists else false</returns>
        public async Task<bool> IsCategoryNameExistsAsync(Category category)
        {
            return await _dbContext.Category.AnyAsync(check => check.CategoryName == category.CategoryName && check.Id != category.Id);
        }
    }
}
