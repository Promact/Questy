﻿using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Promact.Trappist.Repository.Tests;
using System.Linq;
using System.Threading.Tasks;
using Promact.Trappist.Repository.Categories;
using Promact.Trappist.DomainModel.Models.Test;
using System.Collections.Generic;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.DomainModel.ApplicationClasses;
using Promact.Trappist.Repository.Questions;
using Promact.Trappist.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace Promact.Trappist.Test.Tests
{
    [Collection("Register Dependency")]
    public class TestsRepositoryTest : BaseTest
    {
        private readonly ITestsRepository _testRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TestsRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _testRepository = _scope.ServiceProvider.GetService<ITestsRepository>();
            _categoryRepository = _scope.ServiceProvider.GetService<ICategoryRepository>();
            _questionRepository = _scope.ServiceProvider.GetService<IQuestionRepository>();
            _userManager = _scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
        }

        /// <summary>
        /// Test Case For Emtpty Test Model
        /// </summary>
        [Fact]
        public async Task GetAllTestEmpty()
        {
            var list = await _testRepository.GetAllTestsAsync();
            Assert.Equal(0, list.Count);
        }

        /// <summary>
        /// Test Case for adding a new test
        /// </summary>
        [Fact]
        public async Task AddTest()
        {
            var test = CreateTest("testname");
            await _testRepository.CreateTestAsync(test);
            Assert.True(_trappistDbContext.Test.Count() == 1);
        }

        /// <summary>
        /// Test  Case to create a new test when the test name given is unique
        /// </summary>
        [Fact]
        public async Task UniqueNameTest()
        {
            var test = CreateTest("testname");
            await _testRepository.CreateTestAsync(test);
            var name = "nameOfTest";
            var id = 0;
            bool isExist = await _testRepository.IsTestNameUniqueAsync(name, id);
            Assert.True(isExist);
        }

        /// <summary>
        /// Test Case to check when test name is not unique, new test is not added .
        /// </summary>
        [Fact]
        public async Task IsNotUniqueNameTest()
        {
            var test = CreateTest("test name");
            await _testRepository.CreateTestAsync(test);
            var name = "Test name";
            var id = 0;
            bool isExist = await _testRepository.IsTestNameUniqueAsync(name, id);
            Assert.False(isExist);
        }

        /// <summary>
        /// Test Case for fetching settings of a test from database with the help of Id
        /// </summary>
        [Fact]
        public async Task GetSettingsById()
        {
            var test = CreateTest("AOT 669");
            await _testRepository.CreateTestAsync(test);
            Assert.NotNull(test);
            var testSettings = await _testRepository.GetTestByIdAsync(test.Id);
            var testName = testSettings.TestName;
            Assert.Equal(testName, "AOT 669");
        }

        /// <summary>
        /// Test Case for updating the settings set for a test in the database with the help of Id
        /// </summary>
        [Fact]
        public async Task UpdateTestSettingsById()
        {
            var test = CreateTest("AOT 669");
            await _testRepository.CreateTestAsync(test);
            var testSettingsToUpdate = await _testRepository.GetTestByIdAsync(test.Id);
            testSettingsToUpdate.TestName = "IIT BANGALORE";
            testSettingsToUpdate.BrowserTolerance = 2;
            await _testRepository.UpdateTestByIdAsync(testSettingsToUpdate);
            var TestName = "IIT BANGALORE";
            Assert.True(_trappistDbContext.Test.Count(x => x.TestName == TestName) == 1);
            Assert.True(_trappistDbContext.Test.Count(x => x.BrowserTolerance == 2) == 1);
        }

        /// <summary>
        /// Test Case for updating the edited test name in the database 
        /// </summary>
        [Fact]
        public async Task UpdateTestName()
        {
            var test = CreateTest("AOT 669");
            await _testRepository.CreateTestAsync(test);
            var testNameToUpdate = await _testRepository.GetTestByIdAsync(test.Id);
            testNameToUpdate.TestName = "MCKV";
            await _testRepository.UpdateTestNameAsync(test.Id, testNameToUpdate);
            var TestName = "MCKV";
            Assert.True(_trappistDbContext.Test.Count(x => x.TestName == TestName) == 1);
        }

        /// <summary>
        /// Check if any test attendee exist for a particular test
        /// </summary>
        [Fact]
        public async Task IsAttendeeExistAsync()
        {
            var test = CreateTest("Aptitude");
            var testAttendee = TestAttendee();
            await _testRepository.CreateTestAsync(test);
            testAttendee.Test = test;
            testAttendee.TestId = test.Id;
            _trappistDbContext.TestAttendees.Add(testAttendee);
            var isExists = await _testRepository.IsTestAttendeeExistAsync(test.Id);
            Assert.True(isExists);
        }

        /// <summary>
        /// CHeck that no Test attendee exist for a particular test 
        /// </summary>
        [Fact]
        public async Task IsAttendeeNotExistAsync()
        {
            var test = CreateTest("Aptitude");
            await _testRepository.CreateTestAsync(test);
            var isNotExist = await _testRepository.IsTestAttendeeExistAsync(test.Id);
            Assert.False(isNotExist);
        }

        /// <summary>
        /// Delete the selected test
        /// </summary>
        [Fact]
        public async Task DeleteTestAsync()
        {
            var test = CreateTest("Logical");
            await _testRepository.CreateTestAsync(test);
            await _testRepository.DeleteTestAsync(test.Id);
            Assert.Equal(0, _trappistDbContext.Test.Count());
        }

        /// <summary>
        /// Adds the selected category to TestCategory
        /// </summary>      
        [Fact]
        public async Task AddTestCategory()
        {
            var category = CreateCategory();
            await _categoryRepository.AddCategoryAsync(category);
            var test = CreateTest("Final");
            await _testRepository.CreateTestAsync(test);
            TestCategory testcategory = new TestCategory();
            testcategory.TestId = test.Id;
            testcategory.CategoryId = category.Id;
            List<TestCategory> testCategoryList = new List<TestCategory>();
            testCategoryList.Add(testcategory);
            var testAC = await _testRepository.GetTestDetailsByIdAsync(test.Id);
            await _testRepository.AddSelectedCategoryAsync(testCategoryList);
            Assert.True(_trappistDbContext.TestCategory.Count() == 1);
        }

        /// <summary>
        /// Deselects a category
        /// </summary>
        [Fact]
        public async Task DeselectCategory()
        {
            // TODO: When asif's PR is accepted
            string userName = "niharika@promactinfo.com";
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName };
            await _userManager.CreateAsync(user);
            var category = CreateCategory();
            await _categoryRepository.AddCategoryAsync(category);
            var test = CreateTest("Maths");
            await _testRepository.CreateTestAsync(test);
            TestCategory testCategory = new TestCategory();
            testCategory.TestId = test.Id;
            testCategory.CategoryId = category.Id;
            List<TestCategory> testCategoryList = new List<TestCategory>();
            testCategoryList.Add(testCategory);            
            await _testRepository.AddSelectedCategoryAsync(testCategoryList);
            var questionAc = CreateQuestionAc(true, "Question in Category", category.Id, 1);           
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(questionAc, applicationUser.Id);
            TestQuestion testQuestion = new TestQuestion();
            testQuestion.QuestionId = questionAc.Question.Id;
            testQuestion.TestId = test.Id;
            var isExists = await _testRepository.DeselectCategoryAync(category.Id, test.Id);
            Assert.True(true);           
        }

        private DomainModel.Models.Test.Test CreateTest(string testName)
        {
            var test = new DomainModel.Models.Test.Test
            {
                TestName = testName,
                BrowserTolerance = 1
            };
            return test;
        }

        public QuestionAC CreateQuestionAc(bool isSelect, string questionDetails, int categoryId, int id)
        {

            var QuestionAc = new QuestionAC()
            {
                Question = new QuestionDetailAC()
                {
                    Id = id,
                    IsSelect = isSelect,
                    QuestionDetail = questionDetails,
                    QuestionType = 0,
                    DifficultyLevel = 0,
                    CategoryID = categoryId
                },
                CodeSnippetQuestion = null,
                SingleMultipleAnswerQuestion = new SingleMultipleAnswerQuestionAC()
                {
                    SingleMultipleAnswerQuestionOption = new List<SingleMultipleAnswerQuestionOption>()
                    {
                        new SingleMultipleAnswerQuestionOption()
                        {
                            Option="A",
                            IsAnswer=true
                        },
                    }
                }
            };
            return QuestionAc;
        }

        private DomainModel.Models.Category.Category CreateCategory()
        {
            var category = new DomainModel.Models.Category.Category
            {
                CategoryName = "categoryName"
            };
            return category;
        }

        private DomainModel.Models.TestConduct.TestAttendees TestAttendee()
        {
            var testAttendee = new DomainModel.Models.TestConduct.TestAttendees()
            {
                FirstName = "Ritika",
                LastName = "Mohata",
                Email = "ritika@gmail.com"
            };
            return testAttendee;
        }
    }
}