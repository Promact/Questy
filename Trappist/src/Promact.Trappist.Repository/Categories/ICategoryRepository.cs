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
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        /// <summary>
        /// Method to add Category
        /// </summary>
        /// <param name="catagory">category object contains Category details</param>
        /// <returns>category objects contains Category details</returns>
        Task AddCategoryAsync(Category catagory);

        /// <summary>
        /// Method to update Category
        /// </summary>
        /// <param name="categoryToUpdate">category object contains Category details</param>
        /// <returns>category objects contains Category details</returns>
        Task UpdateCategoryAsync(Category categoryToUpdate);

        /// <summary>
        /// Method to check CategoryName exists or not
        /// </summary>
        /// <param name="category">Category object contains Category details</param>
        /// <returns>True If Exists Else False</returns>
        Task<bool> IsCategoryNameExistsAsync(Category category);

        /// <summary>
        /// Method to get Category by id
        /// </summary>
        /// <param name="key">Id which will get Category</param>
        /// <returns>category objects contains Category details</returns>
        Task<Category> GetCategoryByIdAsync(int key);
    }
}