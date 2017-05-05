using Microsoft.Extensions.DependencyInjection;
using Promact.Trappist.DomainModel.ApplicationClasses;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.Repository.Questions;
using Promact.Trappist.Repository.Report;
using Promact.Trappist.Repository.TestConduct;
using Promact.Trappist.Repository.Tests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Promact.Trappist.Test.Reports
{
    [Collection("Register Dependency")]
    public class ReportsRepositoryTest : BaseTest
    {
        private readonly IReportRepository _reportRepository;
        private readonly ITestsRepository _testRepository;
        private readonly ITestConductRepository _testConductRepository;

        public ReportsRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _reportRepository = _scope.ServiceProvider.GetService<IReportRepository>();
            _testRepository = _scope.ServiceProvider.GetService<ITestsRepository>();
            _testConductRepository = _scope.ServiceProvider.GetService<ITestConductRepository>();
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
            var testAttendee = CreateTestAttendee(test.Id);
            var testQuestions =await _reportRepository.GetTestQuestions(test.Id);
            Assert.Equal(2,testQuestions.Count());
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
            var testAnswersList=await _reportRepository.GetTestAttendeeAnswers(testAttendee.Id);
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
                },
                TestLogs = new DomainModel.Models.TestLogs.TestLogs()
                {
                    Id = 1,
                    TestAttendeeId = 1,
                    VisitTestLink = new System.DateTime(2017, 05, 05, 09, 30, 25),
                    FillRegistrationForm = new System.DateTime(2017, 05, 05, 09, 30, 45),
                    PassInstructionpage = new System.DateTime(2017, 05, 05, 09, 45, 45),
                    StartTest = new System.DateTime(2017, 05, 05, 10, 00, 30),
                    FinishTest = new System.DateTime(2017, 05, 05, 12, 30, 30)

                }
            };
            return testAttendee;
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
