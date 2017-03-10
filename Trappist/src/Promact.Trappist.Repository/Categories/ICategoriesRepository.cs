using Promact.Trappist.DomainModel.Models.Category;

namespace Promact.Trappist.Repository.Categories
{
    public interface ICategoriesRepository
    {
        /// <summary>
        /// For Adding a CategoryName into Category Model
        /// </summary>
        /// <param name="catagoryName"></param>
        void CategoryNameAdd(Category catagoryName);
        /// <summary>
        /// will Find a Key of a respective CategoryName
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Category CatagoryIdFind(long key);
        /// <summary>
        /// It will Rename a Category Name
        /// </summary>
        /// <param name="catagoryName"></param>
        void CategoryRename(Category catagoryName);
    }
}
