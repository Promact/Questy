using Promact.Trappist.DomainModel.Models.Category;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.Categories
{
    public interface ICategoriesRepository
    {
        /// <summary>
        /// For Adding a CategoryName into Category Model
        /// </summary>
        /// <param name="catagoryName"></param>
        Task AddCategoryAsync(Category catagory);
        /// <summary>
        /// will Find a Key of a respective CategoryName
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Category GetCatagoryId(long key);
        /// <summary>
        /// It will Rename a Category Name
        /// </summary>
        /// <param name="catagoryName"></param>
        Task CategoryEditAsync(Category catagory);
    }
}
