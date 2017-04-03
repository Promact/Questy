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
        /// Adding a Category in Category model
        /// </summary>
        /// <param name="catagory">Object of class Category</param>
        public void AddCategory(Category category)
        {
            _dbContext.Category.Add(category);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Find a Respective Id from Catagory Table
        /// </summary>
        /// <param name="Key"></param>
        /// <Returns>if key foundthen Return respective category from category table or will return Null</Returns>
        public Category GetCategory(int key)
        {
            return _dbContext.Category.FirstOrDefault(Check => Check.Id == key);
        }

        // <summary>
        // Edit a Category from Category Table
        // </summary>
        // <param name="catagory">object of the class Category</param>
        public void CategoryEdit(Category category)
        {
            _dbContext.Category.Update(category);
            _dbContext.SaveChanges();
        }
    }
}