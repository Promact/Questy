using Promact.Trappist.DomainModel.Models.Category;

namespace Promact.Trappist.Repository.CategoryRepository
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// Add a category in Category Model
        /// </summary>
        /// <param name="catagory"></param>
        void AddCategory(Category catagory);
        /// <summary>
        /// it will search Id and corresponding category
        /// </summary>
        /// <param name="key">unique key of a table</param>
        /// <returns>if Id match then it will return object</returns>
        Category Getcategory(int key);
        /// <summary>
        /// Edit category From Category model
        /// </summary>
        /// <param name="catagory"> Object of class Category</param>
        void CategoryEdit(Category catagory);
        
    }
}
