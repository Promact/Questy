﻿using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Promact.Trappist.Repository.Categories;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using System.Collections.Generic;
using Promact.Trappist.DomainModel.Models.Question;
using Microsoft.AspNetCore.Identity;
using Promact.Trappist.Repository.Questions;
using AutoMapper;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.DomainModel.ApplicationClasses.Test;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.TestLogs;
using System;
using Promact.Trappist.DomainModel.Models;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.Repository.Test;

namespace Promact.Trappist.Test.Tests
{
    [Collection("Register Dependency")]
    public class TestsRepositoryTest : BaseTest
    {
        #region Private Variables
        private readonly ITestsRepository _testRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        #endregion

        //public QuestionAC QuestionAc { get; }
        #region Constructor
        public TestsRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            //QuestionAc = questionAc;
            _testRepository = _scope.ServiceProvider.GetService<ITestsRepository>();
            _categoryRepository = _scope.ServiceProvider.GetService<ICategoryRepository>();
            _questionRepository = _scope.ServiceProvider.GetService<IQuestionRepository>();
            _userManager = _scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            _mapper = _scope.ServiceProvider.GetService<IMapper>();
        }
        #endregion

        #region Get All Test
        /// <summary>
        /// Test Case For Emtpty Test Model
        /// </summary>
        [Fact]
        public async Task GetAllTestEmpty()
        {
            var list = await _testRepository.GetAllTestsAsync();
            Assert.Empty(list);
        }
        #endregion

        #region Add Test
        /// <summary>
        /// Test Case for adding a new test
        /// </summary>
        [Fact]
        public async Task AddTest()
        {
            var test = CreateTest("testname");
            string userName = "suparna@promactinfo.com";
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _testRepository.CreateTestAsync(test, applicationUser.Id);
            Assert.True(_trappistDbContext.Test.Count() == 1);
        }

        /// <summary>
        /// Test  Case to create a new test when the test name given is unique
        /// </summary>
        [Fact]
        public async Task UniqueNameTest()
        {
            var test = CreateTest("testname");
            string userName = "suparna@promactinfo.com";
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _testRepository.CreateTestAsync(test, applicationUser.Id);
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
            string userName = "suparna@promactinfo.com";
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _testRepository.CreateTestAsync(test, applicationUser.Id);
            var name = "Test name";
            var id = 0;
            bool isExist = await _testRepository.IsTestNameUniqueAsync(name, id);
            Assert.False(isExist);
        }
        #endregion

        #region Test Settings
        /// <summary>
        /// Test Case for updating the settings set for a test in the database with the help of Id
        /// </summary>
        [Fact]
        public async Task UpdateTestSettingsById()
        {
            var test = CreateTest("AOT 669");
            string userName = "suparna@promactinfo.com";
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _testRepository.CreateTestAsync(test, applicationUser.Id);
            test.TestName = "IIT BANGALORE";
            test.BrowserTolerance = BrowserTolerance.High;
            int value = (int)test.BrowserTolerance;
            await _testRepository.UpdateTestByIdAsync(test);
            var testName = test.TestName;
            Assert.True(_trappistDbContext.Test.Count(x => x.TestName.Equals(testName)) == 1);
            Assert.True(_trappistDbContext.Test.Count(x => (int)x.BrowserTolerance == value) == 1);
        }

        /// <summary>
        /// Test Case for updating the edited test name in the database 
        /// </summary>
        [Fact]
        public async Task UpdateTestName()
        {
            var test = CreateTest("Computer");
            string userName = "suparna@promactinfo.com";
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _testRepository.CreateTestAsync(test, applicationUser.Id);
            test.TestName = "Maths";
            await _testRepository.UpdateTestNameAsync(test.Id, test);
            var testName = test.TestName;
            Assert.True(_trappistDbContext.Test.Count(x => x.TestName.Equals(testName)) == 1);
        }

        [Fact]
        public async Task IsTestExists()
        {
            string userName = "dasmadhurima48@gmail.com";
            //Configuring Application User
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);

            var category = CreateCategory("category Name");
            await _categoryRepository.AddCategoryAsync(category);
            //Creating Test
            var test = CreateTest("English");
            await _testRepository.CreateTestAsync(test, applicationUser.Id);
            await _testRepository.GetTestByIdAsync(test.Id, applicationUser.Id);
            var result = await _testRepository.IsTestExists(test.Id);
            Assert.True(result);
        }
        #endregion

        #region Pause-Resume Test

        [Fact]
        public async Task PauseResumeTest()
        {
            var test = CreateTest("Test 1");
            string userName = "asif@gmail.com";
            //Configuring Application User
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _testRepository.CreateTestAsync(test, applicationUser.Id);
            await _testRepository.PauseResumeTestAsync(test.Id, true);
            var testObject = _trappistDbContext.Test.First(x => x.Id == test.Id);
            Assert.True(testObject.IsPaused);
        }

        #endregion

        #region Delete Test
        /// <summary>
        /// Check if any test attendee exist for a particular test
        /// </summary>
        [Fact]
        public async Task IsAttendeeExistAsync()
        {
            var test = CreateTest("Aptitude");
            var testAttendee = TestAttendee();
            string userName = "suparna@promactinfo.com";
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _testRepository.CreateTestAsync(test, applicationUser.Id);
            testAttendee.Test = test;
            testAttendee.TestId = test.Id;
            await _trappistDbContext.TestAttendees.AddAsync(testAttendee);
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
            string userName = "suparna@promactinfo.com";
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _testRepository.CreateTestAsync(test, applicationUser.Id);
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
            string userName = "suparna@promactinfo.com";
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _testRepository.CreateTestAsync(test, applicationUser.Id);
            await _testRepository.DeleteTestAsync(test.Id);
            Assert.Equal(0, _trappistDbContext.Test.Count());
        }
        #endregion

        #region Add Test Category
        /// <summary>
        /// Adds the selected category to TestCategory
        /// </summary>      
        [Fact]
        public async Task AddTestCategory()
        {

            var categoryObj = CreateCategory("category1");
            await _categoryRepository.AddCategoryAsync(categoryObj);
            var categoryObject = CreateCategory("category2");
            await _categoryRepository.AddCategoryAsync(categoryObject);
            var testCategoryAC = new List<TestCategoryAC>
            {
                new TestCategoryAC()
                {
                    IsSelect=true,
                    CategoryId=categoryObj.Id
                },new TestCategoryAC()
                {
                    IsSelect=false,
                    CategoryId=categoryObject.Id
                }
            };

            var test = CreateTest("Final");
            string userName = "suparna@promactinfo.com";
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _testRepository.CreateTestAsync(test, applicationUser.Id);
            var testCategoryList = new List<TestCategory>();
            var testCategory = new TestCategory();
            testCategory.TestId = test.Id;
            testCategory.CategoryId = categoryObject.Id;
            testCategoryList.Add(testCategory);
            await _testRepository.AddTestCategoriesAsync(test.Id, testCategoryAC);
            Assert.True(_trappistDbContext.TestCategory.Count() == 1);
            testCategoryAC[0].IsSelect = false;
            await _testRepository.AddTestCategoriesAsync(test.Id, testCategoryAC);
            Assert.True(!_trappistDbContext.TestCategory.Any());
        }

        /// <summary>
        /// Deselects a category
        /// </summary>
        [Fact]
        public async Task DeselectCategory()
        {    //Created application user    
            string userName = "niharika@promactinfo.com";
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var categoryObj = CreateCategory("category1");
            await _categoryRepository.AddCategoryAsync(categoryObj);
            var categoryObject = CreateCategory("category2");
            await _categoryRepository.AddCategoryAsync(categoryObject);
            var testCategoryAc = new List<TestCategoryAC>
            {
                new TestCategoryAC()
                {
                    IsSelect=true,
                    CategoryId=categoryObj.Id
                },new TestCategoryAC()
                {
                    IsSelect=true,
                    CategoryId=categoryObject.Id
                }

            };

            var test = CreateTest("Maths");
            await _testRepository.CreateTestAsync(test, user.Id);
            var testCategoryList = new List<TestCategory>();
            var testCategory = new TestCategory();
            testCategory.TestId = test.Id;
            testCategory.CategoryId = categoryObj.Id;
            testCategoryList.Add(testCategory);
            var testCategoryObj = new TestCategory();
            testCategoryObj.TestId = test.Id;
            testCategoryObj.CategoryId = categoryObject.Id;
            testCategoryList.Add(testCategoryObj);
            await _testRepository.AddTestCategoriesAsync(test.Id, testCategoryAc);
            //creating new question under categoryObj
            var questionAc = CreateQuestionAc(true, "Question in Category", categoryObj.Id, 1);
            var questionAcList = new List<TestQuestionAC>
            {
                new TestQuestionAC()
                {
                    Id=questionAc.Question.Id,
                    IsSelect=true,
                    CategoryID=questionAc.Question.CategoryID
                }

            };
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
        #endregion

        /// <summary>
        /// Test For Adding Questions to TestQuestion Model
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddTestQuestion()
        {

            var category = CreateCategory("Aptitude");
            await _categoryRepository.AddCategoryAsync(category);
            var testCategoryAC = new List<TestCategoryAC>
            {
                new TestCategoryAC()
                {
                    IsSelect=true,
                    CategoryId=category.Id
                }
            };

            var test = CreateTest("Maths");
            string userName = "asif@gmail.com";
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _testRepository.CreateTestAsync(test, applicationUser.Id);
            await _testRepository.AddTestCategoriesAsync(test.Id, testCategoryAC);
            var questionListAc = new List<TestQuestionAC>();
            var question1 = CreateQuestionAc(true, "This will be added..", category.Id, 1);
            questionListAc.Add(new TestQuestionAC()
            {

                Id = question1.Question.Id,
                IsSelect = true,
                CategoryID = question1.Question.CategoryID
            });
            var question2 = CreateQuestionAc(false, "This will not be added.", category.Id, 2);
            questionListAc.Add(new TestQuestionAC()
            {
                Id = question2.Question.Id,
                IsSelect = question2.Question.IsSelect,
                CategoryID = question2.Question.CategoryID
            });
            await _testRepository.AddTestQuestionsAsync(questionListAc, test.Id);
            Assert.True(_trappistDbContext.TestQuestion.Count() == 1);
            var message = await _testRepository.AddTestQuestionsAsync(questionListAc, test.Id);
            Assert.True(message == "No new questions selected..");
            questionListAc[0].IsSelect = false;
            await _testRepository.AddTestQuestionsAsync(questionListAc, test.Id);
            Assert.True(!_trappistDbContext.TestQuestion.Any());
        }

        /// <summary>
        /// Test Case for getting the questions by passing a particular category Id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllTestCategoryQuestionsByIdAsync()
        {

            var category1 = CreateCategory("category1");
            await _categoryRepository.AddCategoryAsync(category1);

            var category2 = CreateCategory("category2");
            await _categoryRepository.AddCategoryAsync(category2);
            var testCategoryAc = new List<TestCategoryAC>
            {
                new()
                {
                    IsSelect=true,
                    CategoryId=category1.Id
                },
                new()
                {
                    IsSelect=false,
                    CategoryId=category2.Id
                }
            };
            string userName = "asif@gmail.com";
            //Configuring Application User
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);

            //Creating questions
            var question1 = CreateQuestionAc(true, "This is in Category 1", category1.Id, 0);
            var question2 = CreateQuestionAc(true, "This is in Category 2", category2.Id, 0);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(question1, applicationUser.Id);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(question2, applicationUser.Id);
            var allQuestions = await _questionRepository.GetAllQuestionsAsync(user.Id, 0, 0, "All", null);
            var test = CreateTest("Maths");
            await _testRepository.CreateTestAsync(test, applicationUser.Id);

            //Adding categories to test
            await _testRepository.AddTestCategoriesAsync(test.Id, testCategoryAc);
            var questionListAc = new List<TestQuestionAC>();
            var questionDetailList = _mapper.Map<List<Question>, List<QuestionDetailAC>>(allQuestions.ToList());
            foreach (var question in questionDetailList)
            {
                var questionAc = new TestQuestionAC();
                questionAc.IsSelect = true;
                questionAc.Id = question.Id;
                questionListAc.Add(questionAc);
            }
            await _testRepository.AddTestQuestionsAsync(questionListAc, test.Id);
            var questionAcList = await _testRepository.GetAllQuestionsByTestIdAsync(test.Id, category1.Id, applicationUser.Id);
            Assert.Single(questionAcList);
            Assert.Equal(2, _trappistDbContext.TestQuestion.Count());
            Assert.True(questionAcList[0].Question.IsSelect);
        }

        #region Get Test Question
        [Fact]
        public async Task GetTestQuestionByTestIdAsyncTest()
        {
            string userName = "deepankar@promactinfo.com";
            var user = new ApplicationUser() { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);

            var categoryObj = CreateCategory("category1");
            await _categoryRepository.AddCategoryAsync(categoryObj);

            var questionAC = CreateQuestionAc(true, "This is some question detail", categoryObj.Id, 1);
            var questionACList = new List<TestQuestionAC>();
            var testQuestionAc = new TestQuestionAC();
            testQuestionAc.Id = questionAC.Question.Id;
            testQuestionAc.CategoryID = questionAC.Question.CategoryID;
            testQuestionAc.IsSelect = questionAC.Question.IsSelect;
            questionACList.Add(testQuestionAc);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(questionAC, applicationUser.Id);

            var test = CreateTest("Maths");
            await _testRepository.CreateTestAsync(test, applicationUser.Id);

            var testQuestion = new TestQuestion();
            testQuestion.QuestionId = questionAC.Question.Id;
            testQuestion.TestId = test.Id;
            await _testRepository.AddTestQuestionsAsync(questionACList, test.Id);

            var questionList = await _testRepository.GetTestQuestionByTestIdAsync(test.Id);

            Assert.True(questionList.Count == 1);
        }
        #endregion

        #region Get Test Details
        /// <summary>
        ///Test case for  getting test details containing categories only
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetTestDetails()
        {
            string userName = "asif@gmail.com";
            //Configuring Application User
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);

            //Creating Category
            var category = CreateCategory("category Name");
            await _categoryRepository.AddCategoryAsync(category);
            var testCategoryAC = new List<TestCategoryAC>
            {
                new TestCategoryAC()
                {
                    IsSelect=true,
                    CategoryId=category.Id
                }
            };

            //Creating Test
            var test = CreateTest("Maths");
            await _testRepository.CreateTestAsync(test, applicationUser.Id);
            await _testRepository.AddTestCategoriesAsync(test.Id, testCategoryAC);
            var testAc = await _testRepository.GetTestByIdAsync(test.Id, applicationUser.Id);
            Assert.True(testAc.CategoryAcList[0].IsSelect);
            await _testRepository.DeleteTestAsync(test.Id);
            var testAcObject = await _testRepository.GetTestByIdAsync(test.Id, applicationUser.Id);
            Assert.Null(testAcObject);
        }
        #endregion

        #region Duplicate Test
        /// <summary>
        /// Test case for duplicating a test
        /// </summary>
        [Fact]
        public async Task DuplicateTest()
        {
            var category1 = CreateCategory("Aptitude");
            var questiontoCreate1 = "Question1";
            var question1 = CreateSingleAnswerQuestion(category1, questiontoCreate1);
            var category2 = CreateCategory("Logical");
            var questiontoCreate2 = "Question2";
            var question2 = CreateSingleAnswerQuestion(category1, questiontoCreate2);
            var oldTest = CreateTest("Maths");
            var testIp = new TestIpAddress();
            testIp.IpAddress = "127.0.0.1";
            testIp.TestId = oldTest.Id;
            string userName = "suparna@promactinfo.com";
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _testRepository.CreateTestAsync(oldTest, applicationUser.Id);
            var testCategoryObject1 = new TestCategory()
            {
                CategoryId = category1.Id,
                TestId = oldTest.Id,
                Test = oldTest
            };
            var testCategoryObject2 = new TestCategory()
            {
                CategoryId = category2.Id,
                TestId = oldTest.Id,
                Test = oldTest
            };
            var testCategoryList = new List<TestCategory>();
            testCategoryList.Add(testCategoryObject1);
            testCategoryList.Add(testCategoryObject2);
            await _trappistDbContext.TestCategory.AddRangeAsync(testCategoryList);
            var testQuestionObject1 = new TestQuestion()
            {
                QuestionId = question1.Id,
                TestId = oldTest.Id,
                Test = oldTest,
            };
            var testQuestionObject2 = new TestQuestion()
            {
                QuestionId = question2.Id,
                TestId = oldTest.Id,
                Test = oldTest,
            };
            var testQuestionList = new List<TestQuestion>();
            testQuestionList.Add(testQuestionObject1);
            testQuestionList.Add(testQuestionObject2);
            await _trappistDbContext.TestQuestion.AddRangeAsync(testQuestionList);
            var testIpAddressObject = new TestIpAddress()
            {
                Test = oldTest,
                TestId = oldTest.Id,
                IpAddress = testIp.IpAddress
            };
            var testIpList = new List<TestIpAddress>();
            testIpList.Add(testIpAddressObject);
            await _trappistDbContext.TestIpAddresses.AddRangeAsync(testIpList);
            var newTest = CreateTest("Maths_Copy");
            await _testRepository.CreateTestAsync(newTest, applicationUser.Id);
            await _testRepository.DuplicateTest(oldTest.Id, newTest);
            var count = await _testRepository.SetTestCopiedNumberAsync(oldTest.TestName);
            Assert.Equal(4, _trappistDbContext.TestQuestion.Count());
            Assert.Equal(4, _trappistDbContext.TestCategory.Count());
            Assert.Equal(2, _trappistDbContext.TestIpAddresses.Count());
            Assert.Equal(1, count);
        }
        #endregion

        #region TestLogs
        /// <summary>
        /// Test case for setting start test log 
        /// </summary>
        [Fact]
        public async Task SetStartTestLog()
        {
            var test = CreateTest("Mathematics");
            await _testRepository.CreateTestAsync(test, "5");
            var testAttendee = CreateTestAttendee(test.Id);
            var testLogs = new TestLogs()
            {
                Id = 1,
                TestAttendeeId = testAttendee.Id,
                VisitTestLink = DateTime.UtcNow,
                FillRegistrationForm = DateTime.UtcNow,
                StartTest = default(DateTime)
            };
            await _trappistDbContext.TestLogs.AddAsync(testLogs);
            await _trappistDbContext.SaveChangesAsync();
            await _testRepository.SetStartTestLogAsync(testAttendee.Id);
            var attendeeStartTestLog = _trappistDbContext.TestLogs.Where(x => x.TestAttendeeId == testAttendee.Id).Select(x => x.StartTest).FirstOrDefault();
            testLogs.StartTest = attendeeStartTestLog;
            Assert.True(testLogs.StartTest != default(DateTime));
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Creates a test
        /// </summary>
        /// <param name="testName">Contains the name of the Test to be created</param>
        /// <returns>Object of Test</returns>
        private DomainModel.Models.Test.Test CreateTest(string testName)
        {
            var test = new DomainModel.Models.Test.Test
            {
                TestName = testName,
                BrowserTolerance = 0,
            };
            return test;
        }

        /// <summary>
        /// Creates category
        /// </summary>
        /// <param name="categoryName">Contains the name of the category to be created</param>
        /// <returns>Object of Category</returns>
        private DomainModel.Models.Category.Category CreateCategory(string categoryName)
        {
            var category = new DomainModel.Models.Category.Category
            {
                CategoryName = categoryName
            };
            return category;
        }

        /// <summary>
        /// Initializes Test Attendees values
        /// </summary>
        /// <returns>Object of TestAttendees</returns>
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
        /// Creates question application class
        /// </summary>
        /// <param name="isSelect">A boolean value indicating a question is being selected or not</param>
        /// <param name="questionDetails">Contains the description of the questions</param>
        /// <param name="categoryId">Contains the Id of the category to which the question is added</param>
        /// <param name="id">Id of the question</param>
        /// <returns>Object of QuestionDetailAC</returns>
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

        /// <summary>
        /// Creates Single Answer Question
        /// </summary>
        /// <param name="categoryToCreate">Contains the name of the category to be created</param>
        /// <param name="question">Contains the details of the question</param>
        /// <returns>Object of QuestionAC</returns>
        private async Task<QuestionAC> CreateSingleAnswerQuestion(DomainModel.Models.Category.Category categoryToCreate, string question)
        {
            var category = await _trappistDbContext.Category.AddAsync(categoryToCreate);
            var singleAnswerQuestion = new QuestionAC()
            {
                Question = new QuestionDetailAC()
                {
                    QuestionDetail = question,
                    CategoryID = category.Entity.Id,
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
                            Option = "A",
                        },
                        new SingleMultipleAnswerQuestionOption()
                        {
                            IsAnswer = false,
                            Option = "B",
                        },
                        new SingleMultipleAnswerQuestionOption()
                        {
                            IsAnswer = false,
                            Option = "C",
                        },
                        new SingleMultipleAnswerQuestionOption()
                        {
                            IsAnswer = false,
                            Option = "D",
                        }
                    }
                }
            };
            return singleAnswerQuestion;
        }

        /// <summary>
        /// Creates test attendee
        /// </summary>
        /// <param name="testId">Contains the Id of the Test taken by the test attendee</param>
        /// <returns>Object of Test Attendees</returns>
        private TestAttendees CreateTestAttendee(int testId)
        {
            var testAttendee = new TestAttendees()
            {
                Id = 1,
                FirstName = "Madhurima",
                LastName = "Das",
                Email = "dasmadhurima@gmail.com",
                RollNumber = "1",
                ContactNumber = "98098776708",
                TestId = testId
            };
            return testAttendee;
        }
        #endregion
    }
}