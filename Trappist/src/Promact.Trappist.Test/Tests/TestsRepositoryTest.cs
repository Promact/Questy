using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Promact.Trappist.Repository.Tests;
using System.Linq;
using System.Threading.Tasks;
using Promact.Trappist.Repository.Categories;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using System.Collections.Generic;
using Promact.Trappist.DomainModel.ApplicationClasses;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.Web.Models;
using Microsoft.AspNetCore.Identity;
using Promact.Trappist.Repository.Questions;
using AutoMapper;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.DomainModel.ApplicationClasses.Test;

namespace Promact.Trappist.Test.Tests
{
    [Collection("Register Dependency")]
    public class TestsRepositoryTest : BaseTest
    {
        private readonly ITestsRepository _testRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public QuestionAC QuestionAc { get; private set; }

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
        /// Test Case for updating the settings set for a test in the database with the help of Id
        /// </summary>
        [Fact]
        public async Task UpdateTestSettingsById()
        {
            var test = CreateTest("AOT 669");
            await _testRepository.CreateTestAsync(test);
            test.TestName = "IIT BANGALORE";
            test.BrowserTolerance = 2;
            await _testRepository.UpdateTestByIdAsync(test);
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
            test.TestName = "MCKV";
            await _testRepository.UpdateTestNameAsync(test.Id, test);
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
            var categoryList = new List<DomainModel.Models.Category.Category>();
            var categoryObj = CreateCategory("category1");
            await _categoryRepository.AddCategoryAsync(categoryObj);
            categoryList.Add(categoryObj);
            var categoryObject = CreateCategory("category2");
            await _categoryRepository.AddCategoryAsync(categoryObject);
            categoryList.Add(categoryObject);
            var categoryListAc = Mapper.Map<List<DomainModel.Models.Category.Category>, List<CategoryAC>>(categoryList);
            var test = CreateTest("Final");
            await _testRepository.CreateTestAsync(test);
            categoryListAc[0].IsSelect = true;
            categoryListAc[1].IsSelect = false;          
            await _testRepository.AddTestCategoriesAsync(test.Id, categoryListAc);
            Assert.True(_trappistDbContext.TestCategory.Count() == 1);
        }

        /// <summary>
        /// Deselects a category
        /// </summary>
        [Fact]
        public async Task DeselectCategory()
        {    //Created application user    
            string userName = "niharika@promactinfo.com";
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName };
            await _userManager.CreateAsync(user);
            var categoryList = new List<DomainModel.Models.Category.Category>();
            var categoryObj = CreateCategory("category1");
            await _categoryRepository.AddCategoryAsync(categoryObj);
            categoryList.Add(categoryObj);
            var categoryObject = CreateCategory("category2");
            await _categoryRepository.AddCategoryAsync(categoryObject);
            categoryList.Add(categoryObject);
            var test = CreateTest("Maths");
            await _testRepository.CreateTestAsync(test);
            var testCategoryList = new List<TestCategory>();
            var testCategory = new TestCategory();
            testCategory.TestId = test.Id;
            testCategory.CategoryId = categoryObj.Id;
            testCategoryList.Add(testCategory);
            var testCategoryObj = new TestCategory();
            testCategoryObj.TestId = test.Id;
            testCategoryObj.CategoryId = categoryObject.Id;           
            testCategoryList.Add(testCategoryObj);
            var categoryListAc = Mapper.Map<List<DomainModel.Models.Category.Category>, List<CategoryAC>>(categoryList);
            categoryListAc[0].IsSelect = true;
            categoryListAc[1].IsSelect = true;
            await _testRepository.AddTestCategoriesAsync(test.Id, categoryListAc) ;
            //creating new question under categoryObj
            var questionAc = CreateQuestionAc(true, "Question in Category", categoryObj.Id, 1);
            var questionAcList = new List<QuestionAC>();
            questionAcList.Add(questionAc);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(questionAc, applicationUser.Id);
            var testQuestion = new TestQuestion();
            testQuestion.QuestionId = questionAc.Question.Id;
            testQuestion.TestId = test.Id;
            await _testRepository.AddTestQuestionsAsync(questionAcList, test.Id);
            //to check if question from a category is added to test it should return true
            var isExists = await _testRepository.DeselectCategoryAync(categoryObj.Id, test.Id);
            Assert.True(isExists);
            //to check if question from a category is not added to test it should return false
            var isQuestionExists = await _testRepository.DeselectCategoryAync(categoryObject.Id, test.Id);
            Assert.False(isQuestionExists);
            //To remove deselected category from TestCategory 
            await _testRepository.RemoveCategoryAndQuestionAsync(testCategory);
            Assert.Equal(1, _trappistDbContext.TestCategory.Count());
            Assert.Equal(0, _trappistDbContext.TestQuestion.Count());
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

            var questionAc = new QuestionAC()
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
            return questionAc;
        }

        private DomainModel.Models.Category.Category CreateCategory(string categoryName)
        {
            var category = new DomainModel.Models.Category.Category
            {
                CategoryName = categoryName
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
        /// <summary>
        /// Test For Adding Questions to TestQuestion Model
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddTestQuestion()
        {
            var categoryList = new List<DomainModel.Models.Category.Category>();           
            var category = CreateCategory("Aptitude");
            await _categoryRepository.AddCategoryAsync(category);
            categoryList.Add(category);
            var categoryAcList = Mapper.Map<List<DomainModel.Models.Category.Category>, List<CategoryAC>>(categoryList);
            var test = CreateTest("Maths");
            await _testRepository.CreateTestAsync(test);
            await _testRepository.AddTestCategoriesAsync(test.Id, categoryAcList);
            var questionListAc = new List<QuestionAC>();
            questionListAc.Add(CreateQuestionAc(true, "This will be added..", category.Id, 1));
            questionListAc.Add(CreateQuestionAc(false, "This will not be added.", category.Id, 2));

            await _testRepository.AddTestQuestionsAsync(questionListAc, test.Id);
            Assert.True(_trappistDbContext.TestQuestion.Count() == 1);
        }
       
        /// <summary>
        /// Test Case for getting the questions by passing a particular category Id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllTestCategoryQuestionsByIdAsync()
        {
            var categoryList = new List<DomainModel.Models.Category.Category>();           
            var category1 = CreateCategory("category1");
            await _categoryRepository.AddCategoryAsync(category1);
            categoryList.Add(category1);
            var category2 = CreateCategory("category2");
            await _categoryRepository.AddCategoryAsync(category2);
            categoryList.Add(category2);
            var categoryAcList = Mapper.Map<List<DomainModel.Models.Category.Category>, List<CategoryAC>>(categoryList);
            string userName = "asifkhan.ak95.ak@gmail.com";           
            //Configuring Application User
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName };
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);

            //Creating questions
            var question1 = CreateQuestionAc(true, "This is in Category 1", category1.Id, 0);
            var question2 = CreateQuestionAc(true, "This is in Category 2", category2.Id, 0);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(question1, applicationUser.Id);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(question2, applicationUser.Id);
            var AllQuestions = await _questionRepository.GetAllQuestionsAsync(user.Id);
            List<Question> ListAC = AllQuestions.ToList();
            var test = CreateTest("Maths");
            await _testRepository.CreateTestAsync(test);

            //Adding categories to test
            await _testRepository.AddTestCategoriesAsync(test.Id, categoryAcList);
            var questionListAc = new List<QuestionAC>();
            var questionDetailList = Mapper.Map<List<Question>, List<QuestionDetailAC>>(ListAC);
            foreach (var question in questionDetailList)
            {
                var questionAc = new QuestionAC();
                question.IsSelect = true;
                questionAc.Question = question;
                questionListAc.Add(questionAc);
            }
            await _testRepository.AddTestQuestionsAsync(questionListAc, test.Id);
            var questionAcList = await _testRepository.GetAllQuestionsByIdAsync(test.Id, category1.Id);
            Assert.Equal(1, questionAcList.Count);
            Assert.Equal(2, _trappistDbContext.TestQuestion.Count());
            Assert.True(questionAcList[0].Question.IsSelect);
        }

        /// <summary>
        ///Test case for  getting test details containing categories only
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetTestDetails()
        {
            var categoryList = new List<DomainModel.Models.Category.Category>();
            //Creating Category
            var category = CreateCategory("category Name");
            await _categoryRepository.AddCategoryAsync(category);
            categoryList.Add(category);
            var categoryAcList = Mapper.Map<List<DomainModel.Models.Category.Category>, List<CategoryAC>>(categoryList);
            //Creating Test
            var test = CreateTest("Maths");
            await _testRepository.CreateTestAsync(test);            
            await _testRepository.AddTestCategoriesAsync(test.Id, categoryAcList);
            var testAc = await _testRepository.GetTestByIdAsync(test.Id);
            Assert.Equal(1, testAc.CategoryAcList.Count());
        }
    }
}