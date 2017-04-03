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
        /// <param name="category">Category object</param>
        /// <returns>Category object</returns>
        public async Task AddCategoryAsync(Category category)
        {
            await _dbContext.Category.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Method to get Category by id
        /// </summary>
        /// <param name="id">Id to get Category</param>
        /// <returns>Category object</returns>
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _dbContext.Category.FindAsync(id);
        }

        /// <summary>
        /// Method to update Category
        /// </summary>
        /// <param name="category">Category object</param>
        /// <returns>Category object</returns>
        public async Task UpdateCategoryAsync(Category category)
        {
            _dbContext.Category.Update(category);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Method to check CategoryName exists or not
        /// </summary>
        /// <param name="category">Category object</param>
        /// <returns>True if exists else flase</returns>
        public async Task<bool> IsCategoryNameExistsAsync(string categoryName, int id)
        {
            return await _dbContext.Category.AnyAsync(x => x.CategoryName.ToLowerInvariant().Equals(categoryName.ToLowerInvariant()) && x.Id != id);
        }
    }
}
