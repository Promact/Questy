using Promact.Trappist.DomainModel.Models.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.Categories
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// Method to get all the Categories
        /// </summary>
        /// <returns>Category list</returns>
        Task <IEnumerable<Category>> GetAllCategoriesAsync();

        /// <summary>
        /// Method to add a Category
        /// </summary>
        /// <param name="catagory">category object contains category details</param>
        Task AddCategoryAsync(Category catagory);

        /// <summary>
        /// Method to Update Category
        /// </summary>
        /// <param name="catagory">category object contains Category details</param>
        /// <returns>category object Contains Category details</returns>
        Task UpdateCategoryAsync(Category categoryToUpdate);

        /// <summary>
        /// Method to Check Same CategoryName Exists or not
        /// </summary>
        /// <param name="categoryName">CategoryName</param>
        /// <returns>true if Exists else False</returns>
        Task<bool> CheckDuplicateCategoryNameAsync(string categoryName);

        /// <summary>
        /// Method to Get Category By its Id
        /// </summary>
        /// <param name="key">id to fing Category</param>
        /// <returns>Category object Contains Category Details</returns>
        Task<Category> GetCategoryByIdAsync(int key);
    }
}