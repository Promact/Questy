using Promact.Trappist.DomainModel.Models.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.Categories
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// Get all Categories
        /// </summary>
        /// <returns>Category list</returns>
        IEnumerable<Category> GetAllCategories();
        /// <summary>
        /// Method to add a Category
        /// </summary>
        /// <param name="catagory">category object contains category details</param>
        Task AddCategoryAsync(Category catagory);
        /// <summary>
        /// Method to Update Category
        /// </summary>
        /// <param name="id">id whose property will be Updated</param>
        /// <param name="catagory">category object contains category details</param>
        Task CategoryUpdateAsync(int id, Category catagory);
        /// <summary>
        /// Method to Check Same CategoryName Exists or not
        /// </summary>
        /// <param name="categoryName">CategoryName</param>
        /// <returns>true if Exists else False</returns>
        Task<bool> CheckDuplicateCategoryNameAsync(string categoryName);
        /// <summary>
        /// Method to Find category of respective id
        /// </summary>
        /// <param name="key">id that will find category</param>
        /// <returns>category object contains category details</returns>
        Task<Category> GetCategoryAsync(int id);
        /// <summary>
        /// Method to check Id is Exists or not
        /// </summary>
        /// <param name="key">take value from Route who id to be search</param>
        /// <returns>true if key found else false</returns>
        Task<bool> SearchForCategoryIdAsync(int key);
    }
}
