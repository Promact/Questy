using Microsoft.AspNetCore.Identity;
using Moq;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Report;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.Repository.Reports;
using Promact.Trappist.Repository.TestConduct;
using Promact.Trappist.Repository.Tests;
using Promact.Trappist.Utility.Constants;
using Promact.Trappist.Utility.GlobalUtil;
using Promact.Trappist.Web.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.ApplicationClasses;
using Promact.Trappist.DomainModel.Models.Question;
using System.Collections.Generic;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.Repository.Questions;
using AutoMapper;
using Promact.Trappist.DomainModel.ApplicationClasses.Test;
using Promact.Trappist.Repository.Categories;
using Promact.Trappist.DomainModel.ApplicationClasses.TestConduct;

namespace Promact.Trappist.Test.Reports
{
    [Collection("Register Dependency")]
    public class ReportsRepositoryTest : BaseTest
    {
        private readonly ITestConductRepository _testConductRepository;
        private readonly IReportRepository _reportRepository;
        private readonly Mock<IGlobalUtil> _globalUtil;
        private readonly IStringConstants _stringConstants;
        private readonly ITestsRepository _testRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _testConductRepository = _scope.ServiceProvider.GetService<ITestConductRepository>();
            _reportRepository = _scope.ServiceProvider.GetService<IReportRepository>();
            _globalUtil = _scope.ServiceProvider.GetService<Mock<IGlobalUtil>>();
            _stringConstants = _scope.ServiceProvider.GetService<IStringConstants>();
            _testRepository = _scope.ServiceProvider.GetService<ITestsRepository>();
            _questionRepository = _scope.ServiceProvider.GetService<IQuestionRepository>();
            _categoryRepository = _scope.ServiceProvider.GetService<ICategoryRepository>();
            _userManager = _scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
        }

        /// <summary>
        /// Test to get test attendee report
        /// </summary>
        [Fact]
        public async Task GetTestAttendeeReportAsyncTest()
        {
            var createTest = await CreateTestAsync();
            var testAttendeeReport = TestAttendeeReport(createTest.Id);
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendeeReport, _stringConstants.MagicString);
            var report = await _reportRepository.GetAllTestAttendeesAsync(createTest.Id);
            Assert.True(report.Count() == 1);
        }

        /// <summary>
        /// Test to set a candidate as starred candidate
        /// </summary>
        [Fact]
        public async Task SetStarredCandidateAsyncTest()
        {
            var createTest = await CreateTestAsync();
            var testAttendeeReport = TestAttendeeReport(createTest.Id);
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendeeReport, _stringConstants.MagicString);
            await _reportRepository.SetStarredCandidateAsync(testAttendeeReport.Id);
            Assert.True(testAttendeeReport.StarredCandidate.Equals(false));
        }

        /// <summary>
        /// Test to set all candidates matching certain criterias as starred candidate
        /// </summary>
        [Fact]
        public async Task SetAllCandidateStarredAsync()
        {
            var createTest = await CreateTestAsync();
            var testAttendeeReport = TestAttendeeReport(createTest.Id);
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendeeReport, _stringConstants.MagicString);
            await _reportRepository.SetAllCandidateStarredAsync(false, 3, "");
            Assert.True(testAttendeeReport.StarredCandidate.Equals(false));
        }

        /// <summary>
        /// Test to fetch a test name
        /// </summary>
        [Fact]
        public async Task GetTestNameAsyncTest()
        {
            var createTest = await CreateTestAsync();
            var testName = await _reportRepository.GetTestNameAsync(createTest.Id);
            Assert.True(testName.TestName.Equals("GK"));
        }

        /// <summary>
        ///This method used to initialize test attendee model parameters.  
        /// </summary>
        /// <returns>It return TestAttendee model object which contain first name,last name,email,contact number,roll number</returns>
        private TestAttendees TestAttendeeReport(int id)
        {
            var testAttendee = new TestAttendees()
            {
                Id = 1,
                FirstName = "Hardik",
                LastName = "Patel",
                Email = "phardi@gmail.com",
                ContactNumber = "1234567890",
                RollNumber = "13it055",
                StarredCandidate = true,
                TestId = id,
                Report = new Report()
                {
                    Percentage = 45.55,
                    Percentile = 46,
                    TotalMarksScored = 45.55,
                    TestStatus = TestStatus.BlockedTest,
                    TimeTakenByAttendee = 45
                }
            };
            return testAttendee;
        }

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
        /// Gets the details of a test attendee along with his marks and test logs by his Id
        /// </summary>
        [Fact]
        public async Task GetTestAttendeeDetailsByIdAsync()
        {
            var test = CreateTest("Mathematics");
            await _testRepository.CreateTestAsync(test, "1");
            var testAttendee = CreateTestAttendee(test.Id);
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, "Added Successfully");
            var testAttendeeObject = await _reportRepository.GetTestAttendeeDetailsByIdAsync(testAttendee.Id);
            Assert.NotNull(testAttendeeObject);
        }

        /// <summary>
        /// Gets all the questions present in a test by test Id
        /// </summary>
        [Fact]
        public async Task GetTestQuestions()
        {
            var test = CreateTest("Mathematics");
            await _testRepository.CreateTestAsync(test, "4");
            var category = CreateCategory("Maths");
            var questionToCreate1 = "Question1";
            var question1 = CreateSingleAnswerQuestion(category, questionToCreate1);

            var questionToCreate2 = "Question2";
            var question2 = CreateSingleAnswerQuestion(category, questionToCreate2);
            var testCategoryObject = new TestCategory()
            {
                CategoryId = category.Id,
                Test = test,
                TestId = test.Id,
                Category = category
            };
            _trappistDbContext.TestCategory.Add(testCategoryObject);
            await _trappistDbContext.SaveChangesAsync();
            var testQuestionObject1 = new TestQuestion()
            {
                QuestionId = question1.Id,
                TestId = test.Id,
                Test = test,

            };
            var testQuestionObject2 = new TestQuestion()
            {
                QuestionId = question2.Id,
                TestId = test.Id,
                Test = test,
            };
            var testQuestionList = new List<TestQuestion>();
            testQuestionList.Add(testQuestionObject1);
            testQuestionList.Add(testQuestionObject2);
            await _trappistDbContext.TestQuestion.AddRangeAsync(testQuestionList);
            await _trappistDbContext.SaveChangesAsync();
            var testQuestions = await _reportRepository.GetTestQuestions(test.Id);
            Assert.Equal(2, testQuestions.Count());
        }

        /// <summary>
        /// Gets all the answers given by a test attendee by his Id
        /// </summary>
        [Fact]
        public async Task GetTestAttendeeAnswers()
        {
            var test = CreateTest("Mathematics");
            await _testRepository.CreateTestAsync(test, "5");
            var testAttendee = CreateTestAttendee(test.Id);
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, "Added");
            var testConduct1 = new DomainModel.Models.TestConduct.TestConduct()
            {
                TestAttendeeId = testAttendee.Id,
                QuestionId = 1
            };
            var testConduct2 = new DomainModel.Models.TestConduct.TestConduct()
            {
                TestAttendeeId = testAttendee.Id,
                QuestionId = 1
            };
            _trappistDbContext.TestConduct.Add(testConduct1);
            _trappistDbContext.TestConduct.Add(testConduct2);
            await _trappistDbContext.SaveChangesAsync();
            var testAnswer1 = new TestAnswers()
            {
                AnsweredOption = 2,
                TestConductId = testConduct1.Id
            };
            var testAnswer2 = new TestAnswers()
            {
                AnsweredOption = 4,
                TestConductId = testConduct2.Id
            };
            _trappistDbContext.TestAnswers.Add(testAnswer1);
            _trappistDbContext.TestAnswers.Add(testAnswer2);
            await _trappistDbContext.SaveChangesAsync();
            var testAnswersList = await _reportRepository.GetTestAttendeeAnswers(testAttendee.Id);
            Assert.Equal(2, testAnswersList.Count());
        }

        private DomainModel.Models.Test.Test CreateTest(string testName)
        {
            var test = new DomainModel.Models.Test.Test
            {
                TestName = testName,
                IncorrectMarks = 1
            };
            return test;
        }

        private TestAttendees CreateTestAttendee(int testId)
        {
            var testAttendee = new TestAttendees()
            {
                Id = 1,
                FirstName = "Ritika",
                LastName = "Mohata",
                Email = "ritika@gmail.com",
                RollNumber = "1",
                TestId = testId,
                Report = new DomainModel.Models.Report.Report()
                {
                    Id = 1,
                    TestAttendeeId = 1,
                    TotalMarksScored = 180,
                    Percentage = 80,
                    Percentile = 75,
                    TestStatus = 0,
                    TimeTakenByAttendee = 150
                }
            };
            return testAttendee;
        }

        [Fact]
        public async Task GetAllAttendeeMarksDetailsAsyncTest()
        {
            //create test
            var createTest = await CreateTestAsync();
            //create category
            var category = CreateCategory("History");
            await _categoryRepository.AddCategoryAsync(category);
            //create question 
            var question = CreateQuestionAc(true, "first Question", category.Id, 1, QuestionType.Multiple);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(question, createTest.CreatedByUserId);
            //add test category
            var categoryList = new List<DomainModel.Models.Category.Category>();
            categoryList.Add(category);
            var categoryListAc = Mapper.Map<List<DomainModel.Models.Category.Category>, List<CategoryAC>>(categoryList);
            categoryListAc[0].IsSelect = true;
            await _testRepository.AddTestCategoriesAsync(createTest.Id, categoryListAc);
            //add test Question
            var questionList = new List<QuestionAC>();
            questionList.Add(question);
            await _testRepository.AddTestQuestionsAsync(questionList, createTest.Id);
            //create test attednee
            var testAttendee = CreateTestAttendee(createTest.Id);
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, _stringConstants.MagicString);
            //AddTestAnswer
            var answer = CreateAnswerAc(question.Question.Id);
            await _testConductRepository.AddAnswerAsync(testAttendee.Id, answer);
            //create test conduct
            var testConduct = new DomainModel.Models.TestConduct.TestConduct()
            {
                Id = 1,
                QuestionId = answer.QuestionId,
                QuestionStatus = answer.QuestionStatus,
                TestAttendeeId = testAttendee.Id
            };
            await _trappistDbContext.TestConduct.AddAsync(testConduct);
            await _trappistDbContext.SaveChangesAsync();
            AddTestAnswer(answer,testConduct.Id);
            var allAttendeeMarksDetails = await _reportRepository.GetAllAttendeeMarksDetailsAsync(createTest.Id);
            var correctAttempted = allAttendeeMarksDetails.First().CorrectQuestionsAttempted;
            var easyQuestionAttempted = allAttendeeMarksDetails.First().EasyQuestionAttempted;
            Assert.Equal(1,easyQuestionAttempted);
            Assert.Equal(0, correctAttempted);
        }

        private TestAnswerAC CreateAnswerAc(int id)
        {
            TestAnswerAC testAnswerAC = new TestAnswerAC()
            {
                QuestionId = id,
                OptionChoice = new List<int>() { 34, 36 },
            };
            
            return testAnswerAC;
        }

        private void  AddTestAnswer(TestAnswerAC answer,int testConductId)
        {
            if (answer.OptionChoice.Count() > 0)
            {
                foreach (var option in answer.OptionChoice)
                {
                    TestAnswers testAnswers = new TestAnswers()
                    {
                        AnsweredOption = option,
                       TestConductId= testConductId
                    };
                    _trappistDbContext.TestAnswers.Add(testAnswers);
                    _trappistDbContext.SaveChanges();
                }
            }
        }

        private QuestionAC CreateQuestionAc(bool isSelect, string questionDetails, int categoryId, int id, QuestionType questionTyp)
        {

            QuestionAC questionAC = new QuestionAC()
            {
                Question = new QuestionDetailAC()
                {
                    Id = id,
                    IsSelect = isSelect,
                    QuestionDetail = questionDetails,
                    QuestionType = questionTyp,
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
                            Id=34,
                            Option="A",
                            IsAnswer=true
                        },
                        new SingleMultipleAnswerQuestionOption()
                        {
                            Id=35,
                            Option="B",
                            IsAnswer=true
                        },
                        new SingleMultipleAnswerQuestionOption()
                        {
                            Id=36,
                            Option="C",
                            IsAnswer=false
                        },
                    }
                }
            };
            return questionAC;
        }

        private DomainModel.Models.Category.Category CreateCategory(string categoryName)
        {
            var category = new DomainModel.Models.Category.Category
            {
                CategoryName = categoryName
            };
            return category;
        }

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
    }
}