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

        void AddCategory(Category catagory);
        
        /// <summary>
        /// it will search Id and corresponding category
        /// </summary>
        /// <param name="key">unique key of a table</param>
        /// <returns>if Id match then it will return object</returns>
        Category GetCategory(int key);
        
        /// <summary>
        /// Edit category From Category model
        /// </summary>
        /// <param name="catagory"> Object of class Category</param>
        void CategoryEdit(Category catagory);
    }
}
