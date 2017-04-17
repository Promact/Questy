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
using Promact.Trappist.DomainModel.ApplicationClasses.Test;
using Promact.Trappist.DomainModel.Models.Test;

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
            _userManager = _scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            _questionRepository = _scope.ServiceProvider.GetService<IQuestionRepository>();

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

        private DomainModel.Models.Test.Test CreateTest(string testName)
        {
            var test = new DomainModel.Models.Test.Test
            {
                TestName = testName,
                BrowserTolerance = 1
            };
            return test;
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
        [Fact]
        public async Task AddTestQuestion()
        {
            TestCategory testCategory = new TestCategory();
            var category = CreateCategory();
            await _categoryRepository.AddCategoryAsync(category);
            
            var test = CreateTest("Maths");
            await _testRepository.CreateTestAsync(test);
            testCategory.TestId = test.Id;
            testCategory.CategoryId = category.Id;
            _trappistDbContext.TestCategory.Add(testCategory);
            await _trappistDbContext.SaveChangesAsync();

            List<QuestionAC> questionListAC = new List<QuestionAC>();
            questionListAC.Add(CreatequestionAC(true, "This will be added..", category.Id , 1));
            questionListAC.Add(CreatequestionAC(false, "This will not be added.", category.Id , 2));

            await _testRepository.AddTestQuestionsAsync(questionListAC, test.Id);
            Assert.True(_trappistDbContext.TestQuestion.Count() == 1);
        }

        public QuestionAC CreatequestionAC(bool isSelect, string questionDetails, int categoryId,int id)
        {

            QuestionAc = new QuestionAC()
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

        [Fact]
        public async Task GetAllTestCategoryQuestionsByIdAsync()
        {
            TestCategory testCategory = new TestCategory();
            var category1 = CreateCategory();   
            await _categoryRepository.AddCategoryAsync(category1);
            var category2 = CreateCategory();
            await _categoryRepository.AddCategoryAsync(category2);
            string userName = "asifkhan.ak95.ak@gmail.com";

          //Configuring Application User
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName };
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);

          //Creating questions
            var question1 = CreatequestionAC(true, "This is in Category 1", category1.Id, 0);
            var question2 = CreatequestionAC(true, "This is in Category 2", category2.Id, 0);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(question1, applicationUser.Id);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(question2, applicationUser.Id);

            var AllQuestions = await _questionRepository.GetAllQuestionsAsync(user.Id);
            List<Question> ListAC = AllQuestions.ToList();
    
            var test = CreateTest("Maths");
            await _testRepository.CreateTestAsync(test);

            //Adding categories to test
            testCategory.TestId = test.Id;
            testCategory.CategoryId = category1.Id;
            await _trappistDbContext.TestCategory.AddAsync(testCategory);
            await _trappistDbContext.SaveChangesAsync();
            testCategory = new TestCategory();
            testCategory.TestId = test.Id;
            testCategory.CategoryId = category2.Id;
            await _trappistDbContext.TestCategory.AddAsync(testCategory);
            await _trappistDbContext.SaveChangesAsync();

            List<QuestionAC> questionListAC = new List<QuestionAC>();
            var questionDetailList = Mapper.Map<List<Question>, List<QuestionDetailAC>>(ListAC);
            foreach (var question in questionDetailList)
            {
                QuestionAC questionAC = new QuestionAC();
                question.IsSelect = true;
                questionAC.Question = question;
                questionListAC.Add(questionAC);
            }
     
            await  _testRepository.AddTestQuestionsAsync(questionListAC, test.Id);
            var questionACList = await _testRepository.GetAllTestCategoryQuestionsByIdAsync(test.Id,category1.Id);
            Assert.Equal(1,questionACList.Count);
            Assert.Equal(2, _trappistDbContext.TestQuestion.Count());
            Assert.True(questionACList[0].Question.IsSelect == true);
        }

        private DomainModel.Models.Category.Category CreateCategory()
        {
            var category = new DomainModel.Models.Category.Category
            {
                CategoryName = "Test Category"
            };
            return category;
        }
        [Fact]
        public async Task GetTestDetails()
        {
            TestAC testAC = new TestAC();
            //Creating Category
            var category = CreateCategory();
            await _categoryRepository.AddCategoryAsync(category);
            //Creating Test
            var test = CreateTest("Maths");
            await _testRepository.CreateTestAsync(test);

            //Adding category to TestCategory model
            var testCategory = new TestCategory()
            {
                TestId = test.Id,
                CategoryId = category.Id
            };
            _trappistDbContext.TestCategory.Add(testCategory);
            await _trappistDbContext.SaveChangesAsync();
            testAC = await _testRepository.GetTestDetailsByIdAsync(test.Id);
            Assert.Equal(1, testAC.CategoryACList.Count());
        }
    }
}