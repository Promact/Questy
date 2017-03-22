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
        /// Method to test get all Categories
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllCategoriesTest()
        {
            var category = CreateCategory();
            await _categoryRepository.AddCategoryAsync(category);
            var result = await _categoryRepository.GetAllCategoriesAsync();
            Assert.True(result.Count() == 1);
        }

        /// <summary>
        /// Method to test add Category
        /// </summary>
        [Fact]
        public async Task AddCategoryTest()
        {
            var category = CreateCategory();
            await _categoryRepository.AddCategoryAsync(category);
            Assert.True(_trappistDbContext.Category.Count() == 1);
        }

        /// <summary>
        /// Method to test get Category by id 
        /// </summary>
        [Fact]
        public async Task GetCategoryByIdTest()
        {
            var category = CreateCategory();
            await _categoryRepository.AddCategoryAsync(category);
            Assert.NotNull(await _categoryRepository.GetCategoryByIdAsync(category.Id));
        }

        /// <summary>
        /// Method to test update Category
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
            Assert.True(_trappistDbContext.Category.Count(x => x.CategoryName.ToLowerInvariant().Equals(categoryToUpdate.CategoryName.ToLowerInvariant())) == 1);
        }

        /// <summary>
        /// Method to test Category Name exists or not
        /// </summary>
        [Fact]
        public async Task IsCategoryNameExistsTest()
        {
            var category = CreateCategory();
            await _categoryRepository.AddCategoryAsync(category);
            var isCategoryExist = CreateCategory();
            isCategoryExist.CategoryName = "Test Category";
            Assert.True(await _categoryRepository.IsCategoryNameExistsAsync(isCategoryExist.CategoryName, isCategoryExist.Id));
        }

        /// <summary>
        /// This is unit testing method. aim of this method is check a category remove from database or not
        /// </summary>
        [Fact]
        public void DeleteCategory()
        {
            var category = CreateCategory();
            var deleteCategory = _categoryRepository.GetCategory(category.Id);
            if (deleteCategory != null)
            {
                _categoryRepository.RemoveCategoryToDatabase(deleteCategory);
                Assert.Equal(0, _trappistDbContext.Category.Count());
            }
        }

        /// <summary>
        /// Method to create a Category object for test
        /// </summary>
        /// <returns>Category object</returns>
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
