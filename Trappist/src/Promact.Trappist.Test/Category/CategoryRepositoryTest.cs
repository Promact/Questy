using Promact.Trappist.DomainModel.DbContext;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System;
using System.Linq;
using Promact.Trappist.Repository.Categories;
using System.Threading.Tasks;

namespace Promact.Trappist.Test.Category
{
    [Collection("Register Dependency")]
    public class CategoryRepositoryTest : BaseTest
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {   
            _categoryRepository = _scope.ServiceProvider.GetService<ICategoryRepository>();
        }

        /// <summary>
        ///Method to test AddCategory Method 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddCategoryAsync()
        {
            var category = CreateCategory();
            await _categoryRepository.AddCategoryAsync(category);
            Assert.True(_trappistDbContext.Category.Count() == 1);
        }

        /// <summary>
        /// Method to test UpdateCategory Method
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateCategoryAsync()
        {
            var category = CreateCategory();
            await _categoryRepository.AddCategoryAsync(category);
            var categoryToUpdate = await _categoryRepository.GetCategoryByIdAsync(category.Id);
            Assert.NotNull(categoryToUpdate);
            if (categoryToUpdate != null)
                categoryToUpdate.CategoryName = "Updated Category";
            await _categoryRepository.UpdateCategoryAsync(categoryToUpdate);
            Assert.True(_trappistDbContext.Category.Count(x=>x.CategoryName == "Updated Category") == 1);
        }

        /// <summary>
        /// Method to Create a Mock object for Test
        /// </summary>
        /// <returns></returns>
        private DomainModel.Models.Category.Category CreateCategory()
        {
            var category = new DomainModel.Models.Category.Category
            {
                CategoryName = "test category",
                CreatedDateTime = DateTime.UtcNow
            };
            return category;
        }
    }
}
