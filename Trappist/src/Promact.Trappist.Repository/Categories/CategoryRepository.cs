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
        /// Method to Add a Category
        /// </summary>
        /// <param name="catagory">category object contains category details</param>
        public async Task AddCategoryAsync(Category category)
        {
            _dbContext.Category.Add(category);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Method to Get Category by its Id
        /// </summary>
        /// <param name="key">id which will Search Category</param>
        /// <returns>category object Contains Category Detaiks</returns>
        public async Task<Category> GetCategoryByIdAsync(int key)
        {
            return await _dbContext.Category.FirstOrDefaultAsync(Check => Check.Id == key);
        }

        /// <summary>
        /// Method to UPdate Category
        /// </summary>
        /// <param name="category">category object Contains Category Details</param>
        public async Task UpdateCategoryAsync(Category category)
        {
            var categoryToUpdate = await GetCategoryByIdAsync(category.Id);
            categoryToUpdate.CategoryName = category.CategoryName;
            _dbContext.Category.Update(categoryToUpdate);
            await _dbContext.SaveChangesAsync();
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