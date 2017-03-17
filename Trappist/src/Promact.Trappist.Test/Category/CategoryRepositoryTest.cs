using Promact.Trappist.DomainModel.DbContext;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System;
using System.Linq;

namespace Promact.Trappist.Test.Category
{
    [Collection("Register Dependency")]
    public class CategoryRepositoryTest
    {
        private readonly Bootstrap _bootstrap;
        private readonly TrappistDbContext _trappistDbContext;

        public CategoryRepositoryTest(Bootstrap bootstrap)
        {
            _bootstrap = bootstrap;
            //resolve dependency to be used in tests
            _trappistDbContext = _bootstrap.ServiceProvider.GetService<TrappistDbContext>();
            ClearDatabase.ClearDatabaseAndSeed(_trappistDbContext);
        }

        [Fact]
        public void AddCategory()
        {
            CreateCategory();
            Assert.True(_trappistDbContext.Category.Count() == 1);
        }

        [Fact]
        public void UpdateCategory()
        {
            var category = CreateCategory();
            var categoryToUpdate = _trappistDbContext.Category.FirstOrDefault(x => x.Id == category.Id);
            Assert.NotNull(categoryToUpdate);
            if (categoryToUpdate != null)
                categoryToUpdate.CategoryName = "Updated Category";
            _trappistDbContext.Category.Update(category);
            _trappistDbContext.SaveChanges();
            Assert.True(_trappistDbContext.Category.Count(x=>x.CategoryName == "Updated Category") == 1);
        }

        private DomainModel.Models.Category.Category CreateCategory()
        {
            var category = new DomainModel.Models.Category.Category
            {
                CategoryName = "test category",
                CreatedDateTime = DateTime.UtcNow
            };
            _trappistDbContext.Category.Add(category);
            _trappistDbContext.SaveChanges();
            return category;
        }
    }
}
