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
        /// Method to add a Category
        /// </summary>
        /// <param name="catagory">category object contains category details</param>
        void CategoryUpdate(int id, Category catagory);
        /// <summary>
        /// Method to Check Same CategoryName Exists or not
        /// </summary>
        /// <param name="categoryName">CategoryName</param>
        /// <returns>true if Exists else False</returns>
        bool CheckDuplicateCategoryName(string categoryName);
        /// <summary>
        /// Find category of respective id
        /// </summary>
        /// <param name="key">id that will find category</param>
        /// <returns>category object contains category details</returns>
        Category GetCategory(int id);
        /// <summary>
        /// Method to check Id is Exists or not
        /// </summary>
        /// <param name="key">take value from Route who id to be search</param>
        /// <returns>true if key found else false</returns>
        bool SearchForCategoryId(int key);
    }
}
