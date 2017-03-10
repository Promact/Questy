using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Category;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.Categories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly TrappistDbContext _dbContext;
        public CategoriesRepository(TrappistDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #region Adding a CategoryName
        /// <summary>
        /// Adding a Category Name Into Category model
        /// </summary>
        /// <param name="catagoryname"></param>
        public async Task AddCategoryAsync(Category category)
        {
            _dbContext.Category.Add(category);
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region Finding a Id Respective CategoryName
        /// <summary>
        /// Findind a Respective key from Catagory Table
        /// </summary>
        /// <param name="Key"></param>
        /// <returns>Category</returns>
        public Category SearchCatagoryId(long key)
        {
            return _dbContext.Category.FirstOrDefault(Check => Check.Id == key);
        }
        #endregion

        #region Edit A Category Name
        // <summary>
        // Updating a Category Name
        // </summary>
        // <param name="catagoryname"></param>
        public async Task CategoryEditAsync(Category category)
        {
            _dbContext.Category.Update(category);
            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}
