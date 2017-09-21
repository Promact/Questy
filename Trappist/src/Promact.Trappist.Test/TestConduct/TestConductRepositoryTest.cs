using AutoMapper;
using CodeBaseSimulator.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Promact.Trappist.DomainModel.ApplicationClasses;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.ApplicationClasses.Test;
using Promact.Trappist.DomainModel.ApplicationClasses.TestConduct;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.Repository.Categories;
using Promact.Trappist.Repository.Questions;
using Promact.Trappist.Repository.TestConduct;
using Promact.Trappist.Repository.Tests;
using Promact.Trappist.Utility.Constants;
using Promact.Trappist.Utility.GlobalUtil;
using Promact.Trappist.Utility.HttpUtil;
using Promact.Trappist.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Xunit;

namespace Promact.Trappist.Test.TestConduct
{
    [Collection("Register Dependency")]
    public class TestConductRepositoryTest : BaseTest
    {
        #region Private Variables
        #region Dependencies
        private readonly ITestConductRepository _testConductRepository;
        private readonly ITestsRepository _testRepository;
        private readonly Mock<IGlobalUtil> _globalUtil;
        private readonly IStringConstants _stringConstants;
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IQuestionRepository _questionRepository;
        private readonly Mock<IHttpService> _httpService;
        #endregion
        #endregion

        #region Constructor
        public TestConductRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _testConductRepository = _scope.ServiceProvider.GetService<ITestConductRepository>();
            _testRepository = _scope.ServiceProvider.GetService<ITestsRepository>();
            _globalUtil = _scope.ServiceProvider.GetService<Mock<IGlobalUtil>>();
            _stringConstants = _scope.ServiceProvider.GetService<IStringConstants>();
            _categoryRepository = _scope.ServiceProvider.GetService<ICategoryRepository>();
            _userManager = _scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            _questionRepository = _scope.ServiceProvider.GetService<IQuestionRepository>();
            _httpService = _scope.ServiceProvider.GetService<Mock<IHttpService>>();
        }
        #endregion

        #region Testing Methods
        /// <summary>
        /// This test case used to check test attendee successfully register.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ValidRegisterTestAttendeesAsync()
        {
            var testAttendee = InitializeTestAttendeeParameters();
            await CreateTestAsync();
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, _stringConstants.MagicString);
            Assert.True(_trappistDbContext.TestAttendees.Count() == 1);
        }

        /// <summary>
        /// This test case used to check test attendee successfully not register.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task InvalidRegisterTestAttendeesAsync()
        {
            var testAttendee = InitializeTestAttendeeParameters();
            await CreateTestAsync();
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, _stringConstants.MagicString);
            var result = await _testConductRepository.IsTestAttendeeExistAsync(testAttendee, _stringConstants.MagicString);
            Assert.True(result);
        }

        /// <summary>
        /// This test case used for check test attendee already exist.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task IsTestAttendeeExist()
        {
            var testAttendee = InitializeTestAttendeeParameters();
            await CreateTestAsync();
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, _stringConstants.MagicString);
            var result = await _testConductRepository.IsTestAttendeeExistAsync(testAttendee, _stringConstants.MagicString);
            Assert.True(result);
        }

        /// <summary>
        /// This test case used for check test attendee not exist.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task IsTestAttendeeNotExist()
        {
            var testAttendee = InitializeTestAttendeeParameters();
            await CreateTestAsync();
            var result = await _testConductRepository.IsTestAttendeeExistAsync(testAttendee, _stringConstants.MagicString);
            Assert.False(result);
        }

        /// <summary>
        /// This test case used to check test magic string is exist.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task IsTestLinkExistAsync()
        {
            var testObject = await CreateTestAsync();
            var testIp = new DomainModel.Models.Test.TestIpAddress();
            testIp.IpAddress = "127.0.0.1";
            testIp.TestId = testObject.Id;
            await _trappistDbContext.TestIpAddresses.AddAsync(testIp);
            var userIp = "127.0.0.1";
            testObject.EndDate = new DateTime(2044, 12, 24);
            testObject.IsLaunched = true;

            var linkExist = await _testConductRepository.IsTestLinkExistForTestConductionAsync(testObject.Link, userIp);
            Assert.True(linkExist);
            testObject.IsPaused = true;
            var linkNotExist = await _testConductRepository.IsTestLinkExistForTestConductionAsync(testObject.Link, userIp);
            Assert.False(linkNotExist);
        }


        /// <summary>
        /// This test case is used to test the test instructions details
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllTestInformationAsync()
        {
            //Creating test
            var test = await CreateTestAsync();
            //Creating test category
            var categoryList = new List<DomainModel.Models.Category.Category>();
            var category1 = CreateCategory("Mathematics");
            await _categoryRepository.AddCategoryAsync(category1);
            var category2 = CreateCategory("Computer");
            categoryList.Add(category1);
            await _categoryRepository.AddCategoryAsync(category2);
            var category3 = CreateCategory("History");
            categoryList.Add(category1);
            await _categoryRepository.AddCategoryAsync(category3);
            categoryList.Add(category2);
            var categoryListAc = Mapper.Map<List<DomainModel.Models.Category.Category>, List<CategoryAC>>(categoryList);
            categoryListAc[0].IsSelect = true;
            categoryListAc[1].IsSelect = false;
            categoryListAc[2].IsSelect = true;
            await _testRepository.AddTestCategoriesAsync(test.Id, categoryListAc);
            //Creating test questions
            var questionList = new List<QuestionAC>
            {
                CreateQuestionAC(true, "Category1 type question", category1.Id, 1),
                CreateQuestionAC(false,"Category1 type question", category1.Id, 2),
                CreateQuestionAC(true,"Category3 type question", category3.Id, 3),
                CreateQuestionAC(true,"Category3 type question", category3.Id, 4),
            };
            var testQuestionList = new List<TestQuestionAC>();
            questionList.ForEach(x =>
            {
                var testQuestion = new TestQuestionAC();
                testQuestion.CategoryID = x.Question.CategoryID;
                testQuestion.Id = x.Question.Id;
                testQuestion.IsSelect = x.Question.IsSelect;
                testQuestionList.Add(testQuestion);
            });
            await _testRepository.AddTestQuestionsAsync(testQuestionList, test.Id);
            var testInstruction = await _testConductRepository.GetTestInstructionsAsync(_stringConstants.MagicString);
            Assert.NotNull(testInstruction);
        }

        /// <summary>
        /// This test case is used to test the test instruction details for invalid testLink
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllNotValidTestInformationAsync()
        {
            var testInstruction = await _testConductRepository.GetTestInstructionsAsync(_stringConstants.MagicString);
            Assert.Null(testInstruction);
        }

        /// <summary>
        /// Test to add answer
        /// </summary>
        [Fact]
        public async Task AddAnswersAsyncTest()
        {
            var testAttendee = InitializeTestAttendeeParameters();
            await CreateTestAsync();
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, _stringConstants.MagicString);
            var attendeeId = await _trappistDbContext.TestAttendees.OrderBy(x => x.Email).Where(x => x.Email.Equals(testAttendee.Email)).Select(x => x.Id).FirstOrDefaultAsync();

            var answer = new TestAnswerAC() { OptionChoice = new List<int>() { 1, 2, 3 } };
            await _testConductRepository.AddAnswerAsync(attendeeId, answer);
            var attendeeAnswer = await _trappistDbContext.AttendeeAnswers.FindAsync(attendeeId);

            Assert.True(attendeeAnswer.Answers != null);

            var newAnswer = new TestAnswerAC() { OptionChoice = new List<int>() { 4, 5 } };
            await _testConductRepository.AddAnswerAsync(attendeeId, newAnswer);

            Assert.True(attendeeAnswer.Answers != null);
        }

        /// <summary>
        /// Test to get answer
        /// </summary>
        [Fact]
        public async Task GetAnswersAsyncTest()
        {
            var testAttendee = InitializeTestAttendeeParameters();
            await CreateTestAsync();
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, _stringConstants.MagicString);
            var attendeeId = await _trappistDbContext.TestAttendees.Where(x => x.Email.Equals(testAttendee.Email)).Select(x => x.Id).FirstOrDefaultAsync();

            var answer = new TestAnswerAC() { OptionChoice = new List<int>() { 1, 2, 3 } };
            await _testConductRepository.AddAnswerAsync(attendeeId, answer);
            var attendeeAnswer = await _testConductRepository.GetAnswerAsync(attendeeId);

            Assert.True(attendeeAnswer.Count == 1);
        }

        /// <summary>
        /// Test to get Test Attendee
        /// </summary>
        [Fact]
        public async Task GetTestAttendeeByIdAsyncTest()
        {
            var testAttendee = InitializeTestAttendeeParameters();
            await CreateTestAsync();
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, _stringConstants.MagicString);
            var attendeeId = await _trappistDbContext.TestAttendees.OrderBy(x => x.Email).Where(x => x.Email.Equals(testAttendee.Email)).Select(x => x.Id).FirstOrDefaultAsync();
            var attendee = await _testConductRepository.GetTestAttendeeByIdAsync(attendeeId);

            Assert.NotNull(attendee);
        }

        /// <summary>
        /// Test to check if Test Attendee exist
        /// </summary>
        [Fact]
        public async Task IsTestAttendeeExistByIdAsyncTest()
        {
            var testAttendee = InitializeTestAttendeeParameters();
            await CreateTestAsync();
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, _stringConstants.MagicString);
            var attendeeId = await _trappistDbContext.TestAttendees.OrderBy(x => x.Email).Where(x => x.Email.Equals(testAttendee.Email)).Select(x => x.Id).FirstOrDefaultAsync();

            Assert.True(await _testConductRepository.IsTestAttendeeExistByIdAsync(attendeeId));
        }

        /// <summary>
        /// Test to set elapsed time 
        /// </summary>
        [Fact]
        public async Task SetElapsedTimeAsyncTest()
        {
            var testAttendee = InitializeTestAttendeeParameters();
            await CreateTestAsync();
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, _stringConstants.MagicString);
            var attendeeId = await _trappistDbContext.TestAttendees.OrderBy(x => x.Email).Where(x => x.Email.Equals(testAttendee.Email)).Select(x => x.Id).FirstOrDefaultAsync();

            //New entry to AttendeeAnswer table will be made since no answer is saved
            await _testConductRepository.SetElapsedTimeAsync(attendeeId, 10);
            //AttendeeAnswer entry will be updated with elapsed time
            await _testConductRepository.SetElapsedTimeAsync(attendeeId, 12);

            var attendeeAnswer = await _trappistDbContext.AttendeeAnswers.FindAsync(attendeeId);
            Assert.NotNull(attendeeAnswer.TimeElapsed);
        }

        /// <summary>
        /// Test to get elapsed time
        /// </summary>
        [Fact]
        public async Task GetElapsedTimeAsyncTest()
        {
            var testAttendee = InitializeTestAttendeeParameters();
            await CreateTestAsync();
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, _stringConstants.MagicString);
            var attendeeId = await _trappistDbContext.TestAttendees.OrderBy(x => x.Email).Where(x => x.Email.Equals(testAttendee.Email)).Select(x => x.Id).FirstOrDefaultAsync();

            //New entry to AttendeeAnswer table will be made since no answer is saved
            await _testConductRepository.SetElapsedTimeAsync(attendeeId, 1);
            //AttendeeAnswer entry will be updated with elapsed time
            await _testConductRepository.SetElapsedTimeAsync(attendeeId, 2);

            var elapsedTime = await _testConductRepository.GetElapsedTimeAsync(attendeeId);
            Assert.True(elapsedTime >= 0.0d);
        }

        /// <summary>
        /// Test to get and set test Attendee's TestStatus
        /// </summary>
        [Fact]
        public async Task GetSetAttendeeTestStatusAsyncTest()
        {
            var testAttendee = InitializeTestAttendeeParameters();
            // Creating test
            var test = await CreateTestAsync();
            //Creating test category
            var categoryList = new List<DomainModel.Models.Category.Category>();
            var category1 = CreateCategory("Mathematics");
            await _categoryRepository.AddCategoryAsync(category1);
            categoryList.Add(category1);
            var categoryListAc = Mapper.Map<List<DomainModel.Models.Category.Category>, List<CategoryAC>>(categoryList);
            categoryListAc[0].IsSelect = true;
            await _testRepository.AddTestCategoriesAsync(test.Id, categoryListAc);

            var question1 = CreateQuestionAC(true, "Category1 type question 1", category1.Id, 1);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(question1, "");
            var question2 = CreateQuestionAC(true, "Category1 type question 2", category1.Id, 2);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(question2, "");

            //Creating test questions
            var questionList = new List<TestQuestionAC>
            {
                new TestQuestionAC(){
                    Id=question1.Question.Id,
                    IsSelect=question1.Question.IsSelect,
                    CategoryID=question1.Question.CategoryID
                },
                new TestQuestionAC()
                {
                     Id=question2.Question.Id,
                    IsSelect=question2.Question.IsSelect,
                    CategoryID=question2.Question.CategoryID

                }
            };
            await _testRepository.AddTestQuestionsAsync(questionList, test.Id);

            testAttendee.Test = test;
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, _stringConstants.MagicString);
            var attendeeId = await _trappistDbContext.TestAttendees.OrderBy(x => x.Email).Where(x => x.Email.Equals(testAttendee.Email)).Select(x => x.Id).FirstOrDefaultAsync();

            var answer = new TestAnswerAC()
            {
                OptionChoice = new List<int>() { question1.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption[0].Id },
                QuestionId = 1,
                QuestionStatus = QuestionStatus.answered
            };
            await _testConductRepository.AddAnswerAsync(attendeeId, answer);

            answer = new TestAnswerAC()
            {
                OptionChoice = new List<int>(),
                QuestionId = 2,
                Code = new Code()
                {
                    Input = "input",
                    Source = "source",
                    Language = ProgrammingLanguage.C
                },
                QuestionStatus = QuestionStatus.unanswered
            };
            await _testConductRepository.AddAnswerAsync(attendeeId, answer);
            //Setting Attendee TestStatus
            await _testConductRepository.SetAttendeeTestStatusAsync(attendeeId, TestStatus.CompletedTest);
            var testStatus = await _testConductRepository.GetAttendeeTestStatusAsync(attendeeId);

            Assert.True(testStatus == TestStatus.CompletedTest);
        }

        [Fact]
        public async Task TransformationTest()
        {
            var testAttendee = InitializeTestAttendeeParameters();
            // Creating test
            var test = await CreateTestAsync();
            //Creating test category
            var categoryList = new List<DomainModel.Models.Category.Category>();
            var category1 = CreateCategory("Mathematics");
            await _categoryRepository.AddCategoryAsync(category1);
            categoryList.Add(category1);
            var categoryListAc = Mapper.Map<List<DomainModel.Models.Category.Category>, List<CategoryAC>>(categoryList);
            categoryListAc[0].IsSelect = true;
            await _testRepository.AddTestCategoriesAsync(test.Id, categoryListAc);

            var question1 = CreateQuestionAC(true, "Category1 type question 1", category1.Id, 1);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(question1, "");
            var question2 = CreateQuestionAC(true, "Category1 type question 2", category1.Id, 2);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(question2, "");

            //Creating test questions
            var questionList = new List<TestQuestionAC>
            {
                new TestQuestionAC(){
                    Id=question1.Question.Id,
                    IsSelect=question1.Question.IsSelect,
                    CategoryID=question1.Question.CategoryID
                },
                new TestQuestionAC()
                {
                     Id=question2.Question.Id,
                    IsSelect=question2.Question.IsSelect,
                    CategoryID=question2.Question.CategoryID

                }
            };
            await _testRepository.AddTestQuestionsAsync(questionList, test.Id);

            testAttendee.Test = test;
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, _stringConstants.MagicString);
            var attendeeId = await _trappistDbContext.TestAttendees.OrderBy(x => x.Email).Where(x => x.Email.Equals(testAttendee.Email)).Select(x => x.Id).FirstOrDefaultAsync();

            var answer = new TestAnswerAC()
            {
                OptionChoice = new List<int>() { question1.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption[0].Id },
                QuestionId = 1,
                QuestionStatus = QuestionStatus.answered
            };
            await _testConductRepository.AddAnswerAsync(attendeeId, answer);

            answer = new TestAnswerAC()
            {
                OptionChoice = new List<int>(),
                QuestionId = 2,
                Code = new Code()
                {
                    Input = "input",
                    Source = "source",
                    Language = ProgrammingLanguage.C
                },
                QuestionStatus = QuestionStatus.unanswered
            };
            await _testConductRepository.AddAnswerAsync(attendeeId, answer);

            //Setting Attendee TestStatus
            await _testConductRepository.SetAttendeeTestStatusAsync(attendeeId, TestStatus.CompletedTest);
            var testStatus = await _trappistDbContext.TestConduct.Where(x => x.TestAttendeeId == attendeeId).ToListAsync();

            Assert.NotNull(testStatus);

        }

        [Fact]
        public async Task ExecuteCodeSnippetAsyncTest()
        {
            var testAttendee = InitializeTestAttendeeParameters();
            // Creating test
            var test = await CreateTestAsync();
            var categoryList = new List<DomainModel.Models.Category.Category>();
            var categoryToCreate = CreateCategory("Coding");
            categoryList.Add(categoryToCreate);
            await _categoryRepository.AddCategoryAsync(categoryToCreate);
            var categoryListAc = Mapper.Map<List<DomainModel.Models.Category.Category>, List<CategoryAC>>(categoryList);
            categoryListAc[0].IsSelect = true;
            await _testRepository.AddTestCategoriesAsync(test.Id, categoryListAc);
            var codingQuestion = CreateCodingQuestion(categoryToCreate);
            await _questionRepository.AddCodeSnippetQuestionAsync(codingQuestion, testAttendee.Email);
            var questionId = (await _trappistDbContext.Question.SingleAsync(x => x.QuestionDetail == codingQuestion.Question.QuestionDetail)).Id;
            //Creating test questions
            var questionList = new List<TestQuestionAC>
            {
                new TestQuestionAC(){
                    Id=codingQuestion.Question.Id,
                    IsSelect=codingQuestion.Question.IsSelect,
                    CategoryID=codingQuestion.Question.CategoryID
                }
            };
            await _testRepository.AddTestQuestionsAsync(questionList, test.Id);

            testAttendee.Test = test;
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, _stringConstants.MagicString);
            var attendeeId = await _trappistDbContext.TestAttendees.OrderBy(x => x.Email).Where(x => x.Email.Equals(testAttendee.Email)).Select(x => x.Id).FirstOrDefaultAsync();

            var answer = new TestAnswerAC()
            {
                OptionChoice = new List<int>(),
                QuestionId = questionId,
                Code = new Code()
                {
                    Input = "input",
                    Source = "source",
                    Language = ProgrammingLanguage.C
                },
                QuestionStatus = QuestionStatus.unanswered
            };

            //Mocking HttpRequest
            var result = new Result()
            {
                CompilationTime = 1,
                CompilerOutput = null,
                CyclicMetrics = 0,
                ExitCode = 0,
                MemoryConsumed = 1,
                Output = "4",
                RunTime = 1
            };
            var serializedResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            _httpService.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(serializedResult, System.Text.Encoding.UTF8, "application/json")
            }));
            //End of Mocking

            var codeRespone = await _testConductRepository.ExecuteCodeSnippetAsync(attendeeId, answer);

            Assert.NotNull(codeRespone);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// This method used for creating a test.
        /// </summary>
        /// <returns></returns>
        private async Task<DomainModel.Models.Test.Test> CreateTestAsync()
        {
            var test = new DomainModel.Models.Test.Test()
            {
                TestName = "GK",
                Duration = 70,
                BrowserTolerance = BrowserTolerance.High,
                CorrectMarks = 4,
                IncorrectMarks = -1
            };
            _globalUtil.Setup(x => x.GenerateRandomString(10)).Returns(_stringConstants.MagicString);
            string userName = "suparna@promactinfo.com";
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName };
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _testRepository.CreateTestAsync(test, applicationUser.Id);
            return test;
        }

        /// <summary>
        ///This method used to initialize test attendee model parameters.  
        /// </summary>
        /// <returns>It return TestAttendee model object which contain first name,last name,email,contact number,roll number</returns>
        private TestAttendees InitializeTestAttendeeParameters()
        {
            var testAttendee = new TestAttendees()
            {
                FirstName = "Hardik",
                LastName = "Patel",
                Email = "phardi@gmail.com",
                ContactNumber = "1234567890",
                RollNumber = "13it055"
            };
            return testAttendee;
        }

        /// <summary>
        /// Creating a category
        /// </summary>
        /// <param categoryName="categoryName">Name of the category</param>
        /// <returns></returns>
        private DomainModel.Models.Category.Category CreateCategory(string categoryName)
        {
            var category = new DomainModel.Models.Category.Category
            {
                CategoryName = categoryName,
                CreatedDateTime = DateTime.UtcNow
            };
            return category;
        }

        /// <summary>
        /// Creating Question application class
        /// </summary>
        /// <param name="isSelect">boolean value to indicate if question is selected or not</param>
        /// <param name="questionDetails">details about a question</param>
        /// <param name="categoryId">categoryid under which the question belongs to</param>
        /// <param name="id">question no</param>
        /// <returns></returns>
        private QuestionAC CreateQuestionAC(bool isSelect, string questionDetails, int categoryId, int id)
        {

            QuestionAC questionAC = new QuestionAC()
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
            return questionAC;
        }

        /// <summary>
        /// Creates Coding Question
        /// </summary>
        /// <returns>Created CodingQuestion object</returns>
        private QuestionAC CreateCodingQuestion(DomainModel.Models.Category.Category categoryToCreate)
        {
            QuestionAC codingQuestion = new QuestionAC
            {
                Question = new QuestionDetailAC
                {
                    QuestionDetail = "<h1>Write a program to add two number</h1>",
                    CategoryID = categoryToCreate.Id,
                    DifficultyLevel = DifficultyLevel.Easy,
                    QuestionType = QuestionType.Programming
                },
                CodeSnippetQuestion = new CodeSnippetQuestionAC
                {
                    CheckCodeComplexity = true,
                    CheckTimeComplexity = true,
                    RunBasicTestCase = true,
                    RunCornerTestCase = false,
                    RunNecessaryTestCase = false,
                    LanguageList = new String[] { "Java", "C" },
                    CodeSnippetQuestionTestCases = new List<CodeSnippetQuestionTestCases>()
                    {
                        new CodeSnippetQuestionTestCases()
                        {
                            TestCaseTitle = "Necessary check",
                            TestCaseDescription = "This case must be successfuly passed",
                            TestCaseMarks = 10.00,
                            TestCaseType = TestCaseType.Necessary,
                            TestCaseInput = "2+2",
                            TestCaseOutput = "4",
                        }
                    }
                },
                SingleMultipleAnswerQuestion = null
            };
            return codingQuestion;
        }
        #endregion
    }
}