using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System;
using System.Linq;
using Promact.Trappist.Repository.Categories;
using System.Threading.Tasks;
using Promact.Trappist.Repository.Questions;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.ApplicationClasses;
using Promact.Trappist.DomainModel.Models.Question;
using System.Collections.Generic;
using Promact.Trappist.Web.Models;
using Microsoft.AspNetCore.Identity;
using Promact.Trappist.Utility.Constants;
using System.Collections;

namespace Promact.Trappist.Test.Category
{
    [Collection("Register Dependency")]
    public class CategoryRepositoryTest : BaseTest
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringConstants _stringConstants;

        public CategoryRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _categoryRepository = _scope.ServiceProvider.GetService<ICategoryRepository>();
            _questionRepository = _scope.ServiceProvider.GetService<IQuestionRepository>();
            _userManager = _scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            _stringConstants = _scope.ServiceProvider.GetService<IStringConstants>();
            ClearDatabase.ClearDatabaseAndSeed(_trappistDbContext);
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
            Assert.True(await _categoryRepository.IsCategoryExistAsync(isCategoryExist.CategoryName, isCategoryExist.Id));
        }

        /// <summary>
        /// Method to test remove Category
        /// </summary>        
        [Fact]
        public async Task DeleteCategory()
        {
            List<string> categoryName = new List<string> { "History", "General Knowledge" };
            var applicationUser = await CreateUserAsync(_stringConstants.UserName);
            var firstCategory = await CreateCategoryAsync(categoryName[0]);
            var SecondCategory = await CreateCategoryAsync(categoryName[1]);
            await CreateSingleAnswerQuestionAsync(SecondCategory.Id, applicationUser.Id);
            bool categoryExistInQuestion = await _categoryRepository.IsCategoryExistInQuestionAsync(SecondCategory.Id);
            Assert.True(categoryExistInQuestion);
            var categoryToDelete = await _categoryRepository.GetCategoryByIdAsync(firstCategory.Id);
            Assert.NotNull(categoryToDelete);
            if (categoryToDelete != null)
            {
                await _categoryRepository.RemoveCategoryAsync(categoryToDelete);
                Assert.Equal(1, _trappistDbContext.Category.Count());
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

        /// <summary>
        /// Creating single answer Question
        /// </summary>
        /// <returns>Object of single answer Question</returns>
        private async Task<QuestionAC> CreateSingleAnswerQuestionAsync(int categoryId, string userId)
        {
            var singleAnswerQuestion = new QuestionAC
            {
                Question = new QuestionDetailAC()
                {
                    QuestionDetail = "what is git?",
                    CategoryID = categoryId,
                    DifficultyLevel = DifficultyLevel.Hard,
                    QuestionType = QuestionType.Single
                },
                SingleMultipleAnswerQuestion = new SingleMultipleAnswerQuestionAC()
                {
                    SingleMultipleAnswerQuestion = new SingleMultipleAnswerQuestion(),
                    SingleMultipleAnswerQuestionOption = new List<SingleMultipleAnswerQuestionOption>()
                    {
                        new SingleMultipleAnswerQuestionOption()
                        {
                            IsAnswer = true,
                            Option = "distributed version control system",
                        },
                        new SingleMultipleAnswerQuestionOption()
                        {
                            IsAnswer = false,
                            Option = "continuous integration service",
                        },
                        new SingleMultipleAnswerQuestionOption()
                        {
                            IsAnswer = false,
                            Option = "Collaborations system",
                        }
                    }
                }
            };
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(singleAnswerQuestion, userId);
            return singleAnswerQuestion;
        }

        /// <summary>
        /// Method to create new user
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>User object</returns>
        public async Task<ApplicationUser> CreateUserAsync(string userName)
        {
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName };
            await _userManager.CreateAsync(user);
            return await _userManager.FindByEmailAsync(user.Email);
        }

        /// <summary>
        /// Method to create new Category
        /// </summary>
        /// <param name="categoryName">Category name</param>
        /// <returns>Category object</returns>
        public async Task<DomainModel.Models.Category.Category> CreateCategoryAsync(string name)
        {
            var category = new DomainModel.Models.Category.Category
            {
                CategoryName = name,
                CreatedDateTime = DateTime.UtcNow
            };
            await _categoryRepository.AddCategoryAsync(category);
            return category;
        }
    }
}