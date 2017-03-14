using Promact.Trappist.DomainModel.DbContext;
using System.Collections.Generic;
using System.Linq;

namespace Promact.Trappist.Repository.Categories
{
    public class CategoryRepository:ICategoryRepository
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
        public IEnumerable<string> GetAllCategories()
        {
            return (_dbContext.Category.Select(x => x.CategoryName).ToList());
        }
    }
}
