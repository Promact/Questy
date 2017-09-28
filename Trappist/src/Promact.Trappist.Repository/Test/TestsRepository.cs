using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.ApplicationClasses;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.ApplicationClasses.Test;
using Promact.Trappist.DomainModel.ApplicationClasses.TestConduct;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Category;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.DomainModel.Models.Test;
using Promact.Trappist.DomainModel.Models.TestLogs;
using Promact.Trappist.Utility.Constants;
using Promact.Trappist.Utility.ExtensionMethods;
using Promact.Trappist.Utility.GlobalUtil;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.Tests
{
    public class TestsRepository : ITestsRepository
    {
        private readonly TrappistDbContext _dbContext;
        private readonly IGlobalUtil _util;
        private readonly IStringConstants _stringConstants;

        public TestsRepository(TrappistDbContext dbContext, IGlobalUtil util, IStringConstants stringConstants)
        {
            _dbContext = dbContext;
            _util = util;
            _stringConstants = stringConstants;
        }

        #region Test 
        public async Task CreateTestAsync(Test test, string userId)
        {
            test.TestName = test.TestName.AllTrim();
            test.Link = _util.GenerateRandomString(10);
            test.StartDate = new DateTime();
            test.EndDate = new DateTime();
            test.CreatedByUserId = userId;
            _dbContext.Test.Add(test);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsTestNameUniqueAsync(string testName, int id)
        {
            var isTestExists = await (_dbContext.Test.AnyAsync(x =>
                                    x.TestName.ToLowerInvariant() == testName.AllTrim().ToLowerInvariant()
                                    && x.Id != id));
            return !isTestExists;
        }

        public async Task<List<TestAC>> GetAllTestsAsync()
        {
            var testAcList = new List<TestAC>();
            TestAC testAcObject;
            var tests = await _dbContext.Test.OrderByDescending(x => x.CreatedDateTime).ToListAsync();
            tests.ForEach(test =>
            {
                testAcObject = new TestAC();
                testAcObject = Mapper.Map<TestAC>(test);
                testAcObject.NumberOfTestAttendees = _dbContext.TestAttendees.Count(x => x.TestId == test.Id);
                testAcObject.NumberOfTestSections = _dbContext.TestCategory.Count(x => x.TestId == test.Id);
                testAcObject.NumberOfTestQuestions = _dbContext.TestQuestion.Count(x => x.TestId == test.Id);
                testAcList.Add(testAcObject);
            });

            return testAcList;
        }
        #endregion

        #region Test Settings
        public async Task UpdateTestNameAsync(int id, Test testObject)
        {
            var testSettingsToUpdate = _dbContext.Test.FirstOrDefault(x => x.Id == id);
            testObject.TestName = testObject.TestName.AllTrim();
            testSettingsToUpdate.TestName = testObject.TestName;
            _dbContext.Test.Update(testSettingsToUpdate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateTestByIdAsync(Test testObject)
        {
            testObject.TestName = testObject.TestName.AllTrim();
            _dbContext.Test.Update(testObject);
            await _dbContext.SaveChangesAsync();
        }
        #region Test-Pause-Resume
        public async Task PauseResumeTestAsync(int id, bool isPause)
        {
            var testObject = await _dbContext.Test.FirstOrDefaultAsync(x => x.Id == id);
            testObject.IsPaused = isPause;
            _dbContext.Test.Update(testObject);
            await _dbContext.SaveChangesAsync();
        }
        #endregion
        public async Task<bool> IsTestExists(int id)
        {
            return await _dbContext.Test.AnyAsync(x => x.Id == id);
        }

        public async Task DeleteIpAddressAsync(int id)
        {
            var ipAddress = await _dbContext.TestIpAddresses.FindAsync(id);
            _dbContext.TestIpAddresses.Remove(ipAddress);
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region Delete Test
        public async Task<bool> IsTestAttendeeExistAsync(int id)
        {
            var test = await _dbContext.Test.Include(x => x.TestAttendees).FirstOrDefaultAsync(x => x.Id == id);
            return test.TestAttendees.Any();
        }

        public async Task DeleteTestAsync(int id)
        {
            Test test = await _dbContext.Test.FindAsync(id);
            _dbContext.Test.Remove(test);
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region Category selection

        public async Task AddTestCategoriesAsync(int testId, List<TestCategoryAC> categoryAcList)
        {
            var testCategoryList = new List<TestCategory>();
            foreach (var categoryAc in categoryAcList)
            {
                // to check whether the category already exists 
                var testCategoryObj = await _dbContext.TestCategory.FirstOrDefaultAsync(x => x.CategoryId == categoryAc.CategoryId && x.TestId == testId);
                // If the category exists and is deselected from the test
                if (testCategoryObj != null && !categoryAc.IsSelect)
                    _dbContext.TestCategory.Remove(testCategoryObj);
                if (testCategoryObj == null && categoryAc.IsSelect)
                {
                    // If category does not exists               
                    var testCategory = new TestCategory();
                    testCategory.TestId = testId;
                    testCategory.CategoryId = categoryAc.CategoryId;
                    //If the category does not exists and is selected by user, then it will be added
                    testCategoryList.Add(testCategory);
                }
            }
            if (testCategoryList.Any())
                await _dbContext.TestCategory.AddRangeAsync(testCategoryList);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeselectCategoryAync(int categoryId, int testId)
        {
            // Listing all the questions category wise.
            var questions = await _dbContext.TestQuestion.Where(x => x.Question.CategoryID == categoryId && x.TestId == testId).ToListAsync();
            return questions.Count != 0;
        }

        public async Task RemoveCategoryAndQuestionAsync(TestCategory testCategory)
        {
            // To find the category to be removed from the test.
            var testCategoryObj = await _dbContext.TestCategory.FirstOrDefaultAsync(x => x.TestId == testCategory.TestId && x.CategoryId == testCategory.CategoryId);
            _dbContext.TestCategory.Remove(testCategoryObj);
            // To find if any question from the removed category is added to the test or not. If any such question found, it is also removed from the test.
            var testQuestions = _dbContext.TestQuestion.Where(x => x.Question.CategoryID == testCategory.CategoryId && x.TestId == testCategory.TestId).ToList();
            if (testQuestions.Any())
                _dbContext.TestQuestion.RemoveRange(testQuestions);
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region Test-Question-Selection
        public async Task<List<QuestionAC>> GetAllQuestionsByIdAsync(int testId, int categoryId, string userId)
        {
            var questionAc = new QuestionAC();
            var questionListAc = new List<QuestionAC>();
            var testQuestions = new List<Question>();
            var testQuestionList = await _dbContext.TestQuestion.Where(x => x.TestId == testId).ToListAsync();
            var singleMultipleQuestions = await _dbContext.SingleMultipleAnswerQuestion.Include(x => x.SingleMultipleAnswerQuestionOption).ToListAsync();
            //Fetches the list of questions from Question Model
            var questionList = await _dbContext.Question.Where(x => x.CategoryID == categoryId && x.CreatedByUserId == userId).ToListAsync();
            //Maps the each question in Question Model to QuestionAC object and make list of type QuestionAC
            questionList.ForEach(question =>
            {
                questionAc = new QuestionAC();
                questionAc.Question = Mapper.Map<QuestionDetailAC>(question);
                questionAc.SingleMultipleAnswerQuestion = Mapper.Map<SingleMultipleAnswerQuestionAC>(singleMultipleQuestions.Find(x => x.Id == question.Id));
                //Checks if the question is already exists in TestQuestion Model,if exists,its IsSelect property made true
                if (testQuestionList.Exists(x => x.QuestionId == questionAc.Question.Id && x.TestId == testId))
                    questionAc.Question.IsSelect = true;
                questionListAc.Add(questionAc);
            });
            return questionListAc;
        }

        public async Task<string> AddTestQuestionsAsync(List<TestQuestionAC> questionsToAdd, int testId)
        {
            bool isDeleted = false;
            var testQuestionList = new List<TestQuestion>();
            var questionsToBeRemoved = new List<TestQuestion>();
            //Adds each question to TestQuestion Model whose IsSelct property is true
            foreach (var questionToAdd in questionsToAdd)
            {
                //Checks if the question exists in TestQuestion for the same test
                var questionExistInTest = await _dbContext.TestQuestion.FirstOrDefaultAsync(x => x.QuestionId == questionToAdd.Id && x.TestId == testId);
                //if question exists in TestQuestion and its IsSelect property is false,then that question will be deleted from TestQuestion
                if (questionExistInTest != null && !questionToAdd.IsSelect)
                {
                    questionsToBeRemoved.Add(questionExistInTest);
                    isDeleted = true;
                }
                //Checks if question's IsSelect property is true
                else if (questionToAdd.IsSelect && questionExistInTest == null)
                {
                    //Creates TestQuestion Object
                    var testQuestionObj = new TestQuestion();
                    testQuestionObj.QuestionId = questionToAdd.Id;
                    testQuestionObj.TestId = testId;
                    //If question is already present in the same Test, it wont be added to TestQuestion 
                    testQuestionList.Add(testQuestionObj);
                }
            }
            //Returns message that user has not selected any new question 
            if (!testQuestionList.Any() && !isDeleted)
                return _stringConstants.NoNewChanges;
            else
            {
                _dbContext.TestQuestion.RemoveRange(questionsToBeRemoved);
                await _dbContext.TestQuestion.AddRangeAsync(testQuestionList);
                await _dbContext.SaveChangesAsync();
                //Returns success message 
                return _stringConstants.SuccessfullySaved;
            }
        }

        public async Task<TestAC> GetTestByIdAsync(int testId, string userId)
        {
            //Find the test by Id from Test Model
            var test = await _dbContext.Test.AsNoTracking().Where(x => x.Id == testId).Include(x => x.TestAttendees).SingleOrDefaultAsync();

            var testIpAddress = await _dbContext.TestIpAddresses.AsNoTracking().Where(x => x.TestId == testId).ToListAsync();
            var testIpAddressAc = Mapper.Map<List<TestIpAddress>, List<TestIpAddressAC>>(testIpAddress);

            //Maps that test with TestAC
            var testAcObject = Mapper.Map<Test, TestAC>(test);

            DateTime currentDate = DateTime.UtcNow;
            string defaultMessage = _stringConstants.WarningMessage;
            int defaultTime = 5;
            int defaultDuration = 60;
            int defaultCorrectMarks = 1;
            DateTime defaultEndDate = currentDate.AddDays(1);
            if (testAcObject != null)
            {
                testAcObject.TestIpAddress = testIpAddressAc;
                testAcObject.NumberOfTestAttendees = test.TestAttendees.Count();
                testAcObject.StartDate = testAcObject.StartDate == default(DateTime) ? currentDate : DateTime.SpecifyKind(testAcObject.StartDate, DateTimeKind.Utc); //If the StartDate field in database contains default value on visiting the Test Settings page of a Test for the first time then that default value gets replaced by current DateTime
                testAcObject.EndDate = testAcObject.EndDate == default(DateTime) ? defaultEndDate : DateTime.SpecifyKind(testAcObject.EndDate, DateTimeKind.Utc); //If the EndDate field in database contains default value on visiting the Test Settings page of a Test for the first time then that default value gets replaced by current DateTime
                testAcObject.Duration = testAcObject.Duration == 0 ? defaultDuration : testAcObject.Duration;
                testAcObject.WarningTime = testAcObject.WarningTime == null ? defaultTime : testAcObject.WarningTime;
                testAcObject.FocusLostTime = testAcObject.FocusLostTime == 0 ? defaultTime : testAcObject.FocusLostTime;
                testAcObject.WarningMessage = testAcObject.WarningMessage == null ? defaultMessage : testAcObject.WarningMessage;
                testAcObject.CorrectMarks = testAcObject.CorrectMarks == 0 ? defaultCorrectMarks : testAcObject.CorrectMarks;
                //Fetches the category list from Category Model
                var categoryList = await _dbContext.Category.AsNoTracking().ToListAsync();
                //Maps Category list to CategoryAC list
                var categoryListAc = Mapper.Map<List<Category>, List<CategoryAC>>(categoryList);
                //Fetches the list of Categories from TestCategory Model
                var testCategoryList = await _dbContext.TestCategory.AsNoTracking().Where(x => x.TestId == testId).Include(x => x.Category).ToListAsync();
                categoryListAc.ForEach(category =>
                {
                    category.QuestionCount = _dbContext.Question.AsNoTracking().Count(x => x.CategoryID == category.Id && x.CreatedByUserId == userId);
                    //If category present in TestCategory Model,then its IsSelect property made true
                    if (testCategoryList.Exists(x => x.CategoryId == category.Id))
                    {
                        category.NumberOfSelectedQuestion = _dbContext.TestQuestion.AsNoTracking().Count(x => x.Question.CategoryID == category.Id && x.TestId == testId);
                        category.IsSelect = true;
                    }
                });
                testAcObject.CategoryAcList = categoryListAc;
                return testAcObject;
            }
            else
                return null;
        }

        public async Task<ICollection<TestConductAC>> GetTestQuestionByTestIdAsync(int testId)
        {
            var questionList = new List<TestConductAC>();
            var singleMultipleQuestions = await _dbContext.SingleMultipleAnswerQuestion.Include(x => x.SingleMultipleAnswerQuestionOption).ToListAsync();
            var codesnippetQuestions = await _dbContext.CodeSnippetQuestion.Include(x => x.QuestionLanguangeMapping).ToListAsync();
            var languages = await _dbContext.CodingLanguage.AsNoTracking().ToListAsync();
            var testQuestionList = await _dbContext.TestQuestion
                 .AsNoTracking()
                 .Where(x => x.TestId == testId).Include(x => x.Question).Include(x => x.Question).ToListAsync();

            foreach (var question in testQuestionList)
            {
                var questionAc = new QuestionAC();
                questionAc.Question = Mapper.Map<QuestionDetailAC>(question.Question);
                questionAc.SingleMultipleAnswerQuestion = Mapper.Map<SingleMultipleAnswerQuestionAC>(singleMultipleQuestions.Find(single => single.Id == question.QuestionId));

                if (question.Question.QuestionType == QuestionType.Programming)
                {
                    var questionCodeSnippet = codesnippetQuestions.Find(z => z.Id == question.QuestionId);
                    questionAc.CodeSnippetQuestion = Mapper.Map<CodeSnippetQuestionAC>(questionCodeSnippet);

                    var languageIds = questionCodeSnippet.QuestionLanguangeMapping.Select(map => map.LanguageId);
                    questionAc.CodeSnippetQuestion.LanguageList = new List<string>();
                    foreach (var id in languageIds)
                        questionAc.CodeSnippetQuestion.LanguageList.Add(languages.Where(lang => lang.Id == id).Select(lang => lang.Language).Single());
                }

                //Removing correct answer(s)
                if (questionAc.SingleMultipleAnswerQuestion != null)
                    questionAc.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption.ForEach(y => y.IsAnswer = false);
                questionList.Add(new TestConductAC() { Question = questionAc, QuestionStatus = QuestionStatus.unanswered });
            }

            return questionList;
        }

        public async Task<TestAC> GetTestByLinkAsync(string link)
        {
            var test = await _dbContext.Test.AsNoTracking().SingleAsync(x => x.Link.Equals(link));
            return await GetTestByIdAsync(test.Id, test.CreatedByUserId);
        }

        public async Task<Test> GetTestSummary(string link)
        {
            return await _dbContext.Test.FirstOrDefaultAsync(x => x.Link == link);
        }
        #endregion

        #region Duplicate Test
        public async Task<Test> DuplicateTest(int testId, Test newTest)
        {
            //Fetch categories present in that particular test and store them in a variable of type list
            var testCategoryList = _dbContext.TestCategory.Where(x => x.TestId == testId);
            // Fetch questions present in that particular test and store them in a variable of type list
            var testQuestionList = _dbContext.TestQuestion.Where(x => x.TestId == testId);
            var test = await _dbContext.Test.FindAsync(newTest.Id);
            //Fetch Ip Addresses in that particular test and store them in a variable of type list
            var testIpAddressList = await _dbContext.TestIpAddresses.Where(x => x.TestId == testId).ToListAsync();

            if (testCategoryList.Any())
            {
                var categoryList = new List<TestCategory>();
                foreach (TestCategory categoryObject in testCategoryList)
                {
                    var categoryObj = new TestCategory();
                    categoryObj.CategoryId = categoryObject.CategoryId;
                    categoryObj.Test = test;
                    categoryObj.TestId = test.Id;
                    categoryList.Add(categoryObj);
                }
                await _dbContext.TestCategory.AddRangeAsync(categoryList);
                await _dbContext.SaveChangesAsync();
                var questionList = new List<TestQuestion>();
                if (testQuestionList.Any())
                {
                    foreach (TestQuestion questionObject in testQuestionList)
                    {
                        var questionObj = new TestQuestion();
                        questionObj.QuestionId = questionObject.QuestionId;
                        questionObj.Test = test;
                        questionObj.TestId = test.Id;
                        questionList.Add(questionObj);
                    }
                    await _dbContext.TestQuestion.AddRangeAsync(questionList);
                    await _dbContext.SaveChangesAsync();
                }
                var ipAddressList = new List<TestIpAddress>();
                if (testIpAddressList.Any())
                {
                    foreach (TestIpAddress testIpAddressObject in testIpAddressList)
                    {
                        var ipAddressObject = new TestIpAddress();
                        ipAddressObject.Test = test;
                        ipAddressObject.TestId = test.Id;
                        ipAddressObject.IpAddress = testIpAddressObject.IpAddress;
                        ipAddressList.Add(ipAddressObject);
                    }
                    await _dbContext.TestIpAddresses.AddRangeAsync(ipAddressList);
                    await _dbContext.SaveChangesAsync();
                }
            }
            return test;
        }
        #endregion

        #region TestLogs
        public async Task SetStartTestLogAsync(int attendeeId)
        {
            var defaultDate = default(DateTime);
            var attendeeLog = await _dbContext.TestLogs.Where(x => x.StartTest == defaultDate && x.TestAttendeeId == attendeeId).FirstOrDefaultAsync();
            if (attendeeLog != null)
            {
                attendeeLog.StartTest = DateTime.UtcNow;
                _dbContext.TestLogs.Update(attendeeLog);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<int> SetTestCopiedNumberAsync(string testName)
        {
            var tests = await _dbContext.Test.Select(x => x.TestName).ToListAsync();
            int count = 0;
            foreach (var test in tests)
            {
                if (test.Contains(testName))
                {
                    if (test == testName)
                        count = count + 1;
                    else if (test == testName + "_copy")
                        count = count + 1;
                    else if (test == testName + "_copy_" + count)
                        count = count + 1;
                }
            }
            return count;
        }
        #endregion
    }
}