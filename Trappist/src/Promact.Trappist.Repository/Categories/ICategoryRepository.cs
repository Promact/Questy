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
        /// Method to check Category Name exists or not
        /// </summary>
        /// <param name="category">Category object</param>
        /// <returns>True if exists else flase</returns>
        Task<bool> IsCategoryNameExistsAsync(string categoryName, int id);

        /// <summary>
        /// Method to get Category by id
        /// </summary>
        /// <param name="id">Id to get Category</param>
        /// <returns>Category object</returns>
        Task<Category> GetCategoryByIdAsync(int id);
        /// <param name="catagory"> Object of class Category</param>
        void CategoryEdit(Category catagory);

        /// <summary>
        /// delete a Category from Category model
        /// </summary>
        /// <param name="catagoryName">object of category model </param>
        Task RemoveCategoryToDatabaseAsync(Category category);
    }
}