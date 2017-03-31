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
        /// Method to Test AddCategoryAsync
        /// </summary>
        [Fact]
        public async Task AddCategoryTest()
        {
            var category = CreateCategory();
            await _categoryRepository.AddCategoryAsync(category);
            Assert.True(_trappistDbContext.Category.Count() == 1);
        }

        /// <summary>
        /// Method to test GetCategoryByIdAsync 
        /// </summary>
        [Fact]
        public async Task GetCategoryByIdTest()
        {
            var category = CreateCategory();
            await _categoryRepository.AddCategoryAsync(category);
            Assert.NotNull(await _categoryRepository.GetCategoryByIdAsync(category.Id));
        }

        /// <summary>
        /// Method to test UpdateCategoryAsync
        /// </summary>
        [Fact]
        public async Task UpdateCategoryTest()
        {
            var category = CreateCategory();
            await _categoryRepository.AddCategoryAsync(category);
            var categoryToUpdate = await _categoryRepository.GetCategoryByIdAsync(category.Id);
            Assert.NotNull(categoryToUpdate);
            if (categoryToUpdate != null)
                categoryToUpdate.CategoryName = "Updated Category";
            await _categoryRepository.UpdateCategoryAsync(categoryToUpdate);
            Assert.True(_trappistDbContext.Category.Count(x => x.CategoryName == "Updated Category") == 1);
        }

        /// <summary>
        /// Method to test IsCategoryNameExistsAsync
        /// </summary>
        [Fact]
        public async Task IsCategoryNameExistsTest()
        {
            var category = CreateCategory();
            await _categoryRepository.AddCategoryAsync(category);
            Assert.False(await _categoryRepository.IsCategoryNameExistsAsync(category));
        }

        /// <summary>
        /// Method to create a Category object for test
        /// </summary>
        /// <returns>category object contains Category details</returns>
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
