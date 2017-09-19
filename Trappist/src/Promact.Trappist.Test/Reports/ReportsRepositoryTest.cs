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
using System;
using CodeBaseSimulator.Models;
using Microsoft.EntityFrameworkCore;

namespace Promact.Trappist.Test.Reports
{
    [Collection("Register Dependency")]
    public class ReportsRepositoryTest : BaseTest
    {
        #region Private Methods
        private readonly ITestConductRepository _testConductRepository;
        private readonly IReportRepository _reportRepository;
        private readonly Mock<IGlobalUtil> _globalUtil;
        private readonly IStringConstants _stringConstants;
        private readonly ITestsRepository _testRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region Constructor
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
        #endregion

        #region Testing Methods
        #region Public Methods
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
        public async Task SetAllCandidateStarredAsyncTest()
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
        /// Gets the details of a test attendee along with his marks and test logs by his Id
        /// </summary>
        [Fact]
        public async Task GetTestAttendeeDetailsByIdAsyncTest()
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
        public async Task GetTestQuestionsTest()
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
        public async Task GetTestAttendeeAnswersTest()
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

        /// <summary>
        /// Calculates the percentile of an attendee
        /// </summary>
        /// <returns>Percentile of an attendee</returns>
        [Fact]
        public async Task CalculatePercentileTest()
        {
            var test = CreateTest("Mathematics");
            await _testRepository.CreateTestAsync(test, "5");
            var testAttendee = CreateTestAttendee(test.Id);
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, "Added");
            var percentile = await _reportRepository.CalculatePercentileAsync(testAttendee.Id, test.Id);
            Assert.Equal(testAttendee.Report.Percentile, percentile);
        }

        /// <summary>
        /// Test case to get all amrks details of all attendees for excel sheet are calculated proper or not
        /// </summary>
        /// <returns>Returns true if return value of actual method is matched with checking value </returns>
        [Fact]
        public async Task GetAllAttendeeMarksDetailsAsyncTest()
        {
            //create test
            var createTest = await CreateTestAsync();
            //create category
            var category = CreateCategory("History");
            await _categoryRepository.AddCategoryAsync(category);
            //create question 
            var question1 = CreateQuestionAc(true, "first Question", category.Id, 1, QuestionType.Multiple);
            var question2 = CreateCodingQuestionAc(true, category.Id, 2, QuestionType.Programming);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(question1, createTest.CreatedByUserId);
            await _questionRepository.AddCodeSnippetQuestionAsync(question2, createTest.CreatedByUserId);
            var questionId1 = (await _trappistDbContext.Question.SingleAsync(x => x.QuestionDetail == question1.Question.QuestionDetail)).Id;
            var questionId2 = (await _trappistDbContext.Question.SingleAsync(x => x.QuestionDetail == question2.Question.QuestionDetail)).Id;
            //add test category
            var categoryList = new List<DomainModel.Models.Category.Category>();
            categoryList.Add(category);
            var testCategoryList = new List<TestCategoryAC>
            {
                new TestCategoryAC
                {
                    CategoryId=category.Id,
                    IsSelect=true,
                }
            };
            //var categoryListAc = Mapper.Map<List<CategoryAC>>(categoryList);
            //categoryListAc[0].IsSelect = true;
            await _testRepository.AddTestCategoriesAsync(createTest.Id, testCategoryList);

            //add test Question
            var questionList = new List<TestQuestionAC>
            {
                new TestQuestionAC()
                {
                    Id = question1.Question.Id,
                    CategoryID = question1.Question.CategoryID,
                    IsSelect = question1.Question.IsSelect
                },
                new TestQuestionAC()
                {
                     Id = question2.Question.Id,
                    IsSelect = question2.Question.IsSelect,
                    CategoryID = question2.Question.CategoryID
                }

            };
            await _testRepository.AddTestQuestionsAsync(questionList, createTest.Id);

            //create test attednee
            var testAttendee = CreateTestAttendee(createTest.Id);
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, _stringConstants.MagicString);
            //AddTestAnswer
            var answer1 = CreateAnswerAc(questionId1);
            await _testConductRepository.AddAnswerAsync(testAttendee.Id, answer1);
            var answer2 = new TestAnswerAC()
            {
                OptionChoice = new List<int>(),
                QuestionId = questionId2,
                Code = new Code()
                {
                    Input = "input",
                    Source = "source",
                    Language = ProgrammingLanguage.C
                },
                QuestionStatus = QuestionStatus.answered
            };
            await _testConductRepository.AddAnswerAsync(testAttendee.Id, answer2);

            //create test conduct
            var testConduct1 = new DomainModel.Models.TestConduct.TestConduct()
            {
                Id = 1,
                QuestionId = answer1.QuestionId,
                QuestionStatus = answer1.QuestionStatus,
                TestAttendeeId = testAttendee.Id
            };
            var testConduct2 = new DomainModel.Models.TestConduct.TestConduct()
            {
                Id = 2,
                QuestionId = answer2.QuestionId,
                QuestionStatus = answer2.QuestionStatus,
                TestAttendeeId = testAttendee.Id
            };
            await _trappistDbContext.TestConduct.AddAsync(testConduct1);
            await _trappistDbContext.TestConduct.AddAsync(testConduct2);
            await _trappistDbContext.SaveChangesAsync();
            AddTestAnswer(answer1, testConduct1.Id);
            AddTestAnswer(answer2, testConduct2.Id);
            //add test code solution
            var codeSolution1 = new TestCodeSolution()
            {
                TestAttendeeId = testAttendee.Id,
                QuestionId = questionId2,
                Solution = answer2.Code.Source,
                Language = answer2.Code.Language,
                Score = 1
            };
            var codeSolution2 = new TestCodeSolution()
            {
                TestAttendeeId = testAttendee.Id,
                QuestionId = questionId2,
                Solution = answer2.Code.Source,
                Language = answer2.Code.Language,
                Score = 0
            };
            await _trappistDbContext.TestCodeSolution.AddAsync(codeSolution1);
            await _trappistDbContext.TestCodeSolution.AddAsync(codeSolution2);
            await _trappistDbContext.SaveChangesAsync();
            var allAttendeeMarksDetails = await _reportRepository.GetAllAttendeeMarksDetailsAsync(createTest.Id);
            var totalQuestionAttempted = allAttendeeMarksDetails.First().NoOfQuestionAttempted;
            var easyQuestionAttempted = allAttendeeMarksDetails.First().EasyQuestionAttempted;
            Assert.Equal(2, easyQuestionAttempted);
            Assert.Equal(2, totalQuestionAttempted);           
        }

        /// <summary>
        /// Test case for getting the number of questions attended by a test attendee
        /// </summary>
        [Fact]
        public async Task GetAttemptedQuestionsByAttendeeAsyncTest()
        {
            var test = CreateTest("General Awareness");
            await _testRepository.CreateTestAsync(test, "5");
            var testAttendee = CreateTestAttendee(test.Id);
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, "Added");
            var testConduct1 = new DomainModel.Models.TestConduct.TestConduct()
            {
                TestAttendeeId = testAttendee.Id,
                QuestionId = 1,
                QuestionStatus = QuestionStatus.answered
            };
            var testConduct2 = new DomainModel.Models.TestConduct.TestConduct()
            {
                TestAttendeeId = testAttendee.Id,
                QuestionId = 1,
                QuestionStatus = QuestionStatus.review
            };
            var testConduct3 = new DomainModel.Models.TestConduct.TestConduct()
            {
                TestAttendeeId = testAttendee.Id,
                QuestionId = 1,
                QuestionStatus = QuestionStatus.review
            };
            _trappistDbContext.TestConduct.Add(testConduct1);
            _trappistDbContext.TestConduct.Add(testConduct2);
            _trappistDbContext.TestConduct.Add(testConduct3);
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
            var testAnswer3 = new TestAnswers()
            {
                AnsweredOption = null,
                TestConductId = testConduct3.Id
            };
            _trappistDbContext.TestAnswers.Add(testAnswer1);
            _trappistDbContext.TestAnswers.Add(testAnswer2);
            _trappistDbContext.TestAnswers.Add(testAnswer3);
            await _trappistDbContext.SaveChangesAsync();
            var numberOfQuestionsAttemptedByAttendee = await _reportRepository.GetAttemptedQuestionsByAttendeeAsync(testAttendee.Id);
            Assert.Equal(2, numberOfQuestionsAttemptedByAttendee);
        }

        /// <summary>
        /// Test case for getting the total marks scored by an attendee in code snippet question
        /// </summary>
        [Fact]
        public async Task GetTotalMarksOfCodeSnippetQuestionAsyncTest()
        {
            var test = CreateTest("Programming");
            await _testRepository.CreateTestAsync(test, "5");
            var testAttendee = CreateTestAttendee(test.Id);
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, "Added");
            //create category
            var category = CreateCategory("programming");
            await _categoryRepository.AddCategoryAsync(category);
            //create coding question 
            var question = CreateCodingQuestionAc(true, category.Id, 2, QuestionType.Programming);
            await _questionRepository.AddCodeSnippetQuestionAsync(question, test.CreatedByUserId);
            var questionId = (await _trappistDbContext.Question.SingleAsync(x => x.QuestionDetail == question.Question.QuestionDetail)).Id;
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
                QuestionStatus = QuestionStatus.answered
            };
            await _testConductRepository.AddAnswerAsync(testAttendee.Id, answer);
            //add test code solution
            var codeSolution1 = new TestCodeSolution()
            {
                Id = 1,
                TestAttendeeId = testAttendee.Id,
                QuestionId = questionId,
                Solution = answer.Code.Source,
                Language = answer.Code.Language,
                Score = 1
            };
            var codeSolution2 = new TestCodeSolution()
            {
                Id = 2,
                TestAttendeeId = testAttendee.Id,
                QuestionId = questionId,
                Solution = answer.Code.Source,
                Language = answer.Code.Language,
                Score = 0
            };
            await _trappistDbContext.TestCodeSolution.AddAsync(codeSolution1);
            await _trappistDbContext.TestCodeSolution.AddAsync(codeSolution2);
            await _trappistDbContext.SaveChangesAsync();
            var marksScoredInCodeSnippetQuestion = await _reportRepository.GetTotalMarksOfCodeSnippetQuestionAsync(testAttendee.Id, questionId);
            var marksScoredWhenQuestionIdAbsent = await _reportRepository.GetTotalMarksOfCodeSnippetQuestionAsync(testAttendee.Id, 3);
            Assert.Equal(3, marksScoredInCodeSnippetQuestion);
            Assert.Equal(-1, marksScoredWhenQuestionIdAbsent);
        }

        /// <summary>
        /// Test case for getting the details of test code solution of a code snippet question
        /// </summary>
        [Fact]
        public async Task GetTestCodeSolutionDetailsAsyncTest()
        {
            var test = CreateTest("Coding Test");
            await _testRepository.CreateTestAsync(test, "5");
            var testAttendee = CreateTestAttendee(test.Id);
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, "Added");
            //create category
            var category = CreateCategory("programming");
            await _categoryRepository.AddCategoryAsync(category);
            //create coding question 
            var question = CreateCodingQuestionAc(true, category.Id, 2, QuestionType.Programming);
            await _questionRepository.AddCodeSnippetQuestionAsync(question, test.CreatedByUserId);
            var questionId = (await _trappistDbContext.Question.SingleAsync(x => x.QuestionDetail == question.Question.QuestionDetail)).Id;
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
                QuestionStatus = QuestionStatus.answered
            };
            await _testConductRepository.AddAnswerAsync(testAttendee.Id, answer);
            //add test code solution
            var codeSolution1 = new TestCodeSolution()
            {
                Id = 1,
                TestAttendeeId = testAttendee.Id,
                QuestionId = questionId,
                Solution = answer.Code.Source,
                Language = answer.Code.Language,
                Score = 1
            };
            var codeSolution2 = new TestCodeSolution()
            {
                Id = 2,
                TestAttendeeId = testAttendee.Id,
                QuestionId = questionId,
                Solution = answer.Code.Source,
                Language = answer.Code.Language,
                Score = 0
            };
            await _trappistDbContext.TestCodeSolution.AddAsync(codeSolution1);
            await _trappistDbContext.TestCodeSolution.AddAsync(codeSolution2);
            await _trappistDbContext.SaveChangesAsync();
            var codeSolutionObject = await _reportRepository.GetTestCodeSolutionDetailsAsync(testAttendee.Id, 3);
            var codeSolutionAcObject = await _reportRepository.GetTestCodeSolutionDetailsAsync(testAttendee.Id, questionId);
            Assert.Equal(null, codeSolutionObject);
            Assert.Equal(answer.Code.Language, codeSolutionAcObject.Language);
            Assert.Equal(answer.Code.Source, codeSolutionAcObject.CodeSolution);
            Assert.Equal(1, codeSolutionAcObject.NumberOfSuccessfulAttempts);
            Assert.Equal(2, codeSolutionAcObject.TotalNumberOfAttempts);
        }

        /// <summary>
        /// Test case for getting the details of code snippet question
        /// </summary>
        [Fact]
        public async Task GetCodeSnippetDetailsAsync()
        {
            var test = CreateTest("Coding Test1");
            await _testRepository.CreateTestAsync(test, "5");
            var testAttendee = CreateTestAttendee(test.Id);
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, "Added");
            //create category
            var category = CreateCategory("coding question");
            await _categoryRepository.AddCategoryAsync(category);
            //create coding question 
            var question = CreateCodingQuestionAc(true, category.Id, 2, QuestionType.Programming);
            await _questionRepository.AddCodeSnippetQuestionAsync(question, test.CreatedByUserId);
            var questionId = (await _trappistDbContext.Question.SingleAsync(x => x.QuestionDetail == question.Question.QuestionDetail)).Id;

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
                QuestionStatus = QuestionStatus.answered
            };
            await _testConductRepository.AddAnswerAsync(testAttendee.Id, answer);
            var testCaseResultList = new List<TestCaseResult>();
            //add test code solution
            var codeSolution1 = new TestCodeSolution()
            {
                Id = 1,
                TestAttendeeId = testAttendee.Id,
                QuestionId = questionId,
                Solution = answer.Code.Source,
                Language = answer.Code.Language,
                Score = 1
            };
            var codeSolution2 = new TestCodeSolution()
            {
                Id = 2,
                TestAttendeeId = testAttendee.Id,
                QuestionId = questionId,
                Solution = answer.Code.Source,
                Language = answer.Code.Language,
                Score = 0
            };
            await _trappistDbContext.TestCodeSolution.AddAsync(codeSolution1);
            await _trappistDbContext.TestCodeSolution.AddAsync(codeSolution2);
            await _trappistDbContext.SaveChangesAsync();
            //add test case result
            var testCaseResult1 = new TestCaseResult()
            {
                Id = 1,
                Processing = 180,
                Memory = 50,
                Output = "5",
                CodeSnippetQuestionTestCasesId = question.CodeSnippetQuestion.CodeSnippetQuestionTestCases[0].CodeSnippetQuestionId,
                TestCodeSolutionId = codeSolution1.Id
            };
            var testCaseResult2 = new TestCaseResult()
            {
                Id = 2,
                Processing = 190,
                Memory = 70,
                Output = "Hello World",
                CodeSnippetQuestionTestCasesId = question.CodeSnippetQuestion.CodeSnippetQuestionTestCases[0].CodeSnippetQuestionId,
                TestCodeSolutionId = codeSolution2.Id
            };
            await _trappistDbContext.TestCaseResult.AddAsync(testCaseResult1);
            await _trappistDbContext.TestCaseResult.AddAsync(testCaseResult2);
            await _trappistDbContext.SaveChangesAsync();
            testCaseResultList.Add(testCaseResult1);
            testCaseResultList.Add(testCaseResult2);
            codeSolution1.TestCaseResultCollection = testCaseResultList;
            codeSolution2.TestCaseResultCollection = testCaseResultList;
            var codeSnippetDetailsObject = await _reportRepository.GetCodeSnippetDetailsAsync(testAttendee.Id, questionId);
            Assert.Equal(2, codeSnippetDetailsObject.Count());
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// This method is used to create a test with the given testname
        /// </summary>
        /// <param name="testName">Name of the test that needs to be created</param>
        /// <returns>Returns the object of type Test</returns>
        private DomainModel.Models.Test.Test CreateTest(string testName)
        {
            var test = new DomainModel.Models.Test.Test
            {
                TestName = testName,
                CorrectMarks = 3,
                IncorrectMarks = 1
            };
            return test;
        }

        // <summary>
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
        /// This method is used to create a test attendee with attendee details under a test
        /// </summary>
        /// <param name="testId">Id of the test</param>
        /// <returns>returns the object of type TestAttendees</returns>
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
                    Percentile = 50,
                    TestStatus = 0,
                    TimeTakenByAttendee = 150
                },
            };
            return testAttendee;
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

       

        private TestAnswerAC CreateAnswerAc(int id)
        {
            TestAnswerAC testAnswerAC = new TestAnswerAC()
            {
                QuestionId = id,
                OptionChoice = new List<int>() { 34, 36 },
                QuestionStatus = QuestionStatus.answered
            };
            return testAnswerAC;
        }

        /// <summary>
        /// This method is used to add the created answer to database
        /// </summary>
        /// <param name="answer">Object of TestAnswerAC type</param>
        /// <param name="testConductId">Test conduct id</param>
        private void AddTestAnswer(TestAnswerAC answer, int testConductId)
        {
            if (answer.OptionChoice.Count() > 0)
            {
                foreach (var option in answer.OptionChoice)
                {
                    TestAnswers testAnswers = new TestAnswers()
                    {
                        AnsweredOption = option,
                        TestConductId = testConductId
                    };
                    _trappistDbContext.TestAnswers.Add(testAnswers);
                    _trappistDbContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// This method will create a question with question details 
        /// </summary>
        /// <param name="isSelect">Boolean value to set the question as selected question or not</param>
        /// <param name="questionDetails">Description of the Question</param>
        /// <param name="categoryId">Id of the category</param>
        /// <param name="id">Id of the question</param>
        /// <param name="questionTyp">Type of the question</param>
        /// <returns>Returns the object of type QuestionAC</returns>
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

        /// <summary>
        /// Create a category and return the category object
        /// </summary>
        /// <param name="categoryName">Name of the category</param>
        /// <returns>Returns the object of created category</returns>
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

        /// <summary>
        /// This method is used to create a coding type quetion 
        /// </summary>
        /// <param name="isSelect">Boolean value to set the question as selected question or not</param>
        /// <param name="categoryId">Id of the category</param>
        /// <param name="id">Id of the question</param>
        /// <param name="questionType">Type of the question</param>
        /// <returns></returns>
        private QuestionAC CreateCodingQuestionAc(bool isSelect, int categoryId, int id, QuestionType questionType)
        {
            QuestionAC questionAC = new QuestionAC()
            {
                Question = new QuestionDetailAC()
                {
                    Id = id,
                    IsSelect = isSelect,
                    QuestionDetail = "<h1>Write a program to add two number</h1>",
                    QuestionType = questionType,
                    DifficultyLevel = 0,
                    CategoryID = categoryId
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
            return questionAC;
        }
        #endregion
        #endregion
    }
}