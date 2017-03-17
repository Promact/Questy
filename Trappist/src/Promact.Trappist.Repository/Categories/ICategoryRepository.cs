using Promact.Trappist.DomainModel.Models.Category;
using System.Collections.Generic;
namespace Promact.Trappist.Repository.Categories
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// Get all Categories
        /// </summary>
        /// <returns>Category list</returns>
        IEnumerable<Category> GetAllCategories();

        void AddCategory(Category catagory);
        /// <summary>
        /// Edit category From Category model
        /// </summary>
        /// <param name="catagory"> Object of class Category</param>
        void CategoryEdit(int id, Category catagory);
        /// <summary>
        /// Check for Category name is Exists or not
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns>true if Exists else false</returns>
        bool CheckDuplicateCategoryName(string categoryName);
        /// <summary>
        /// will find Category by Resprctive Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Category GetCategory(int id);

    }
}
