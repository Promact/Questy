using Promact.Trappist.Repository.TestConduct;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.Repository.Tests;
using Moq;
using Promact.Trappist.Utility.GlobalUtil;
using System.Linq;
using Promact.Trappist.Utility.Constants;
using Promact.Trappist.DomainModel.ApplicationClasses.TestConduct;
using Promact.Trappist.DomainModel.ApplicationClasses;
using Promact.Trappist.DomainModel.Models.Question;
using System.Collections.Generic;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Models.Test;
using System;
using Promact.Trappist.Repository.Categories;
using Promact.Trappist.DomainModel.ApplicationClasses.Test;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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

            TestInstructionsAC testInstruction = new TestInstructionsAC();
            testInstruction = await _testConductRepository.GetTestInstructionsAsync(_stringConstants.MagicString);
            Assert.NotNull(testInstruction);
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
            var attendeeId = await _trappistDbContext.TestAttendees.Where(x=>x.Email==testAttendee.Email).Select(x=>x.Id).FirstOrDefaultAsync();
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
            var attendeeId = await _trappistDbContext.TestAttendees.Where(x => x.Email == testAttendee.Email).Select(x => x.Id).FirstOrDefaultAsync();
            var answer = "This is an answer";
            await _testConductRepository.AddAnswerAsync(attendeeId, answer);
            var attendeeAnswer = await _testConductRepository.GetAnswerAsync(attendeeId);

            Assert.True(attendeeAnswer.Equals(answer));
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
                WarningTime = 2,
                CorrectMarks = 4,
                IncorrectMarks = -1,
                BrowserTolerance = 1

            };
            _globalUtil.Setup(x => x.GenerateRandomString(10)).Returns(_stringConstants.MagicString);
            await _testRepository.CreateTestAsync(test);
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