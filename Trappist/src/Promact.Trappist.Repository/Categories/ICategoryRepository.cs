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
        /// <param name="catagory">Category object</param>
        /// <returns>Category object</returns>
        Task AddCategoryAsync(Category catagory);

        /// <summary>
        /// Method to update Category
        /// </summary>
        /// <param name="categoryToUpdate">Category object</param>
        /// <returns>Category object</returns>
        Task UpdateCategoryAsync(Category categoryToUpdate);

        /// <summary>
        /// Method to check category exists or not
        /// </summary>
        /// <param name="categoryName">CategoryName of the Category</param>
        /// <param name="id">Id of the Category</param>
        /// <returns>>True if exists else false</returns>
        Task<bool> IsCategoryExistAsync(string categoryName, int id);

        /// <summary>
        /// Method to get Category by id
        /// </summary>
        /// <param name="id">Id to get Category</param>
        /// <returns>Category object</returns>
        Task<Category> GetCategoryByIdAsync(int id);

        /// <summary>
        /// Method to delete Category
        /// </summary>
        /// <param name="catagory">Category object</param>
        Task DeleteCategoryAsync(Category category);

        /// <summary>
        /// Method to check Category exist in Question or not
        /// </summary>
        /// <param name="categoryId">Id to check Category</param>
        /// <returns>True if exist else false</returns>
        Task<bool> IsCategoryExistInQuestionAsync(int id);
    }
}