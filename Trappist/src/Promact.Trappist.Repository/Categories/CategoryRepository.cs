using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Category;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Promact.Trappist.Utility.ExtensionMethods;

namespace Promact.Trappist.Repository.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        #region Dependency
        private readonly TrappistDbContext _dbContext;
        #endregion

        #region Constructor
        public CategoryRepository(TrappistDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Public Methods
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return (await _dbContext.Category.OrderByDescending(g => g.CreatedDateTime).ToListAsync());
        }

        public async Task AddCategoryAsync(Category category)
        {
            category.CategoryName = category.CategoryName.AllTrim();
            await _dbContext.Category.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _dbContext.Category.FindAsync(id);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            category.CategoryName = category.CategoryName.AllTrim();
            _dbContext.Category.Update(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsCategoryExistAsync(string categoryName, int id)
        {
            categoryName = categoryName.AllTrim();
            return await _dbContext.Category.AnyAsync(x => x.CategoryName.ToLowerInvariant().Equals(categoryName.ToLowerInvariant()) && x.Id != id);
        }

        public async Task RemoveCategoryAsync(Category category)
        {
            _dbContext.Category.Remove(category);
            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}