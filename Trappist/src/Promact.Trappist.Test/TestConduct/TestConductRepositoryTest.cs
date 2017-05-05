using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Promact.Trappist.DomainModel.ApplicationClasses;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.ApplicationClasses.Test;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.Repository.Categories;
using Promact.Trappist.Repository.TestConduct;
using Promact.Trappist.Repository.Tests;
using Promact.Trappist.Utility.Constants;
using Promact.Trappist.Utility.GlobalUtil;
using Promact.Trappist.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            await CreateTestAsync();
            var result = await _testConductRepository.IsTestLinkExistAsync(_stringConstants.MagicString);
            Assert.True(result);
        }

        /// <summary>
        /// This test case used to check test magic string is not exist.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task IsTestLinkNotExistAsync()
        {
            var result = await _testConductRepository.IsTestLinkExistAsync(_stringConstants.MagicString);
            Assert.False(result);
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
            await _testRepository.AddTestQuestionsAsync(questionList, test.Id);
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

            var answer = "This is an answer";
            await _testConductRepository.AddAnswerAsync(attendeeId, answer);
            var attendeeAnswer = await _trappistDbContext.AttendeeAnswers.FindAsync(attendeeId);

            Assert.True(attendeeAnswer.Answers.Equals(answer));

            var newAnswer = "This is a new answer";
            await _testConductRepository.AddAnswerAsync(attendeeId, newAnswer);

            Assert.True(attendeeAnswer.Answers.Equals(newAnswer));
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
            //JSON string
            var answer = "[{ \"questionId\":1,\"optionChoice\":[1],\"code\":null,\"questionStatus\":1}]";
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
            await _testConductRepository.SetElapsedTimeAsync(attendeeId);
            //AttendeeAnswer entry will be updated with elapsed time
            await _testConductRepository.SetElapsedTimeAsync(attendeeId);

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
            await _testConductRepository.SetElapsedTimeAsync(attendeeId);
            //AttendeeAnswer entry will be updated with elapsed time
            await _testConductRepository.SetElapsedTimeAsync(attendeeId);

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
            await CreateTestAsync();
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, _stringConstants.MagicString);
            var attendeeId = await _trappistDbContext.TestAttendees.OrderBy(x => x.Email).Where(x => x.Email.Equals(testAttendee.Email)).Select(x => x.Id).FirstOrDefaultAsync();

            //Setting Attendee TestStatus
            await _testConductRepository.SetAttendeeTestStatusAsync(attendeeId, TestStatus.CompletedTest);
            var testStatus = await _testConductRepository.GetAttendeeTestStatusAsync(attendeeId);

            Assert.True(testStatus == TestStatus.CompletedTest);            
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
        #endregion
    }
}