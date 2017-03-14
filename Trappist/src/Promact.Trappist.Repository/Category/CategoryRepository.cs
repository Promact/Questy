using Promact.Trappist.DomainModel.DbContext;
using System.Linq;

namespace Promact.Trappist.Repository.Category
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
        public void AddCategory(DomainModel.Models.Category.Category category)
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
        /// <Returns>will Return respective category from category table</Returns>
        public DomainModel.Models.Category.Category GetId(int key)
        {
            return _dbContext.Category.FirstOrDefault(Check => Check.Id == key);
        }
        #endregion

        #region Edit A Category Name
        // <summary>
        // Edit a Category from Category Table
        // </summary>
        // <param name="catagory">object of the class Category</param>
        public void CategoryEdit(DomainModel.Models.Category.Category category)
        {
            _dbContext.Category.Update(category);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}
