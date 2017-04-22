using Promact.Trappist.DomainModel.DbContext;
using System.Linq;
using Promact.Trappist.DomainModel.Models.Test;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.Utility.GlobalUtil;
using System;
using AutoMapper;
using Promact.Trappist.DomainModel.Models.Category;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.DomainModel.ApplicationClasses.Test;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.ApplicationClasses;
using Promact.Trappist.Utility.ExtensionMethods;
using Promact.Trappist.Utility.Constants;
using System.Globalization;

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
        /// <summary>
        /// this method is used to create a new test
        /// </summary>
        /// <param name="test">object of Test</param>
        public async Task CreateTestAsync(Test test)
        {
            test.TestName = test.TestName.AllTrim();
            test.Link = _util.GenerateRandomString(10);
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

        public async Task<List<Test>> GetAllTestsAsync()
        {
            return await _dbContext.Test.OrderByDescending(x => x.CreatedDateTime).ToListAsync();
        }
        #endregion
        #region Test Settings
        /// <summary>
        /// Updates the edited Test Name
        /// </summary>
        /// <param name="id">The parameter "id" takes takes the value of the Id from the route</param>
        /// <param name="testObject">The parameter "testObject" is an object of Test</param>
        /// <returns>Updated Test Name</returns>
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

        public async Task<bool> IsTestExists(int id)
        {
            return await _dbContext.Test.AnyAsync(x => x.Id == id);
        }
        #endregion
        #region Delete Test
        public async Task<bool> IsTestAttendeeExistAsync(int id)
        {
            Test test = await _dbContext.Test.Include(x => x.TestAttendees).FirstOrDefaultAsync(x => x.Id == id);
            return test.TestAttendees.Any();
        }

        public async Task DeleteTestAsync(int id)
        {
            Test test = await _dbContext.Test.FindAsync(id);
            _dbContext.Test.Remove(test);
            await _dbContext.SaveChangesAsync();
        }
        #endregion
        #region Test-Question-Selection
        public async Task<List<QuestionAC>> GetAllQuestionsByIdAsync(int testId, int categoryId)
        {
            var questionAc = new QuestionAC();
            var questionListAc = new List<QuestionAC>();
            var testQuestionList = await _dbContext.TestQuestion.Where(x => x.TestId == testId).ToListAsync();
            //Fetches the list of questions from Question Model
            var questionList = await _dbContext.Question.Where(x => x.CategoryID == categoryId).Include(y => y.SingleMultipleAnswerQuestion).ThenInclude(x => x.SingleMultipleAnswerQuestionOption).ToListAsync();
            //Maps the each question in Question Model to QuestionAC object and make list of type QuestionAC
            questionList.ForEach(question =>
            {
                questionAc = new QuestionAC();
                questionAc.Question = Mapper.Map<Question, QuestionDetailAC>(question);
                questionAc.SingleMultipleAnswerQuestion = Mapper.Map<SingleMultipleAnswerQuestion, SingleMultipleAnswerQuestionAC>(question.SingleMultipleAnswerQuestion);
                //Checks if the question is already exists in TestQuestion Model,if exists,its IsSelect property made true
                if (testQuestionList.Exists(x => x.QuestionId == questionAc.Question.Id && x.TestId == testId))
                    questionAc.Question.IsSelect = true;
                questionListAc.Add(questionAc);
            });
            return questionListAc;
        }

        public async Task<string> AddTestQuestionsAsync(List<QuestionAC> questionsToAdd, int testId)
        {
            bool isDeleted = false;
            var testQuestionList = new List<TestQuestion>();
            //Adds each question to TestQuestion Model whose IsSelct property is true
            foreach (var questionToAdd in questionsToAdd)
            {
                //Checks if the question exists in TestQuestion for the same test
                var questionExistInTest = await _dbContext.TestQuestion.FirstOrDefaultAsync(x => x.QuestionId == questionToAdd.Question.Id && x.TestId == testId);
                //if question exists in TestQuestion and its IsSelect property is false,then that question will be deleted from TestQuestion
                if (questionExistInTest != null && !questionToAdd.Question.IsSelect)
                {
                    _dbContext.TestQuestion.Remove(questionExistInTest);
                    await _dbContext.SaveChangesAsync();
                    isDeleted = true;
                }
                //Checks if question's IsSelect property is true
                else if (questionToAdd.Question.IsSelect)
                {
                    //Creates TestQuestion Object
                    var testQuestionObj = new TestQuestion();
                    testQuestionObj.QuestionId = questionToAdd.Question.Id;
                    testQuestionObj.TestId = testId;
                    //If question is already present in the same Test, it wont be added to TestQuestion 
                    if (questionExistInTest == null)
                        testQuestionList.Add(testQuestionObj);
                }
            }
            //Returns message that user has not selected any new question 
            if (!testQuestionList.Any() && !isDeleted)
                return _stringConstants.NoNewChanges;
            else
            {
                await _dbContext.TestQuestion.AddRangeAsync(testQuestionList);
                await _dbContext.SaveChangesAsync();
                //Returns success message 
                return _stringConstants.SuccessfullySaved;
            }
        }

        public async Task<TestAC> GetTestByIdAsync(int testId)
        {
            //Find the test by Id from Test Model
            var test = await _dbContext.Test.FindAsync(testId);
            //Maps that test with TestAC
            var testAcObject = Mapper.Map<Test, TestAC>(test);
            string currentDate = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            DateTime date = DateTime.ParseExact(currentDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            if (testAcObject != null)
            {
                testAcObject.StartDate = testAcObject.StartDate == default(DateTime) ? date : testAcObject.StartDate; //If the StartDate field in database contains default value on visiting the Test Settings page of a Test for the first time then that default value gets replaced by current DateTime
                testAcObject.EndDate = testAcObject.EndDate == default(DateTime) ? date : testAcObject.EndDate; //If the EndDate field in database contains default value on visiting the Test Settings page of a Test for the first time then that default value gets replaced by current DateTime

                //Fetches the category list from Category Model
                var categoryList = await _dbContext.Category.ToListAsync();
                //Maps Category list to CategoryAC lis
                var categoryListAc = Mapper.Map<List<Category>, List<CategoryAC>>(categoryList);
                //Fetches the list of Categories from TestCategory Model
                var testCategoryList = await _dbContext.TestCategory.Where(x => x.TestId == testId).Include(x => x.Category).ToListAsync();
                categoryListAc.ForEach(category =>
                {
                    //If category present in TestCategory Model,then its IsSelect property made true
                    if (testCategoryList.Exists(x => x.CategoryId == category.Id))
                        category.IsSelect = true;
                });
                testAcObject.CategoryAcList = categoryListAc;
                return testAcObject;
            }
            else
                return null;
        }
        #endregion
        #region Test-Question-Selection
        public async Task<List<QuestionAC>> GetAllQuestionsByIdAsync(int testId, int categoryId)
        {
            var questionAc = new QuestionAC();
            var questionListAc = new List<QuestionAC>();
            var testQuestionList = await _dbContext.TestQuestion.Where(x => x.TestId == testId).ToListAsync();
            //Fetches the list of questions from Question Model
            var questionList = await _dbContext.Question.Where(x => x.CategoryID == categoryId).Include(y => y.SingleMultipleAnswerQuestion).ThenInclude(x => x.SingleMultipleAnswerQuestionOption).ToListAsync();
            //Maps the each question in Question Model to QuestionAC object and make list of type QuestionAC
            questionList.ForEach(question =>
            {
                questionAc = new QuestionAC();
                questionAc.Question = Mapper.Map<Question, QuestionDetailAC>(question);
                questionAc.SingleMultipleAnswerQuestion = Mapper.Map<SingleMultipleAnswerQuestion, SingleMultipleAnswerQuestionAC>(question.SingleMultipleAnswerQuestion);
                //Checks if the question is already exists in TestQuestion Model,if exists,its IsSelect property made true
                if (testQuestionList.Exists(x => x.QuestionId == questionAc.Question.Id && x.TestId == testId))
                    questionAc.Question.IsSelect = true;
                questionListAc.Add(questionAc);
            });
            return questionListAc;
        }

        public async Task<string> AddTestQuestionsAsync(List<QuestionAC> questionsToAdd, int testId)
        {
            bool isDeleted = false;
            var testQuestionList = new List<TestQuestion>();
            //Adds each question to TestQuestion Model whose IsSelct property is true
            foreach (var questionToAdd in questionsToAdd)
            {
                //Checks if the question exists in TestQuestion for the same test
                var questionExistInTest = await _dbContext.TestQuestion.FirstOrDefaultAsync(x => x.QuestionId == questionToAdd.Question.Id && x.TestId == testId);
                //if question exists in TestQuestion and its IsSelect property is false,then that question will be deleted from TestQuestion
                if (questionExistInTest != null && !questionToAdd.Question.IsSelect)
                {
                    _dbContext.TestQuestion.Remove(questionExistInTest);
                    await _dbContext.SaveChangesAsync();
                    isDeleted = true;
                }
                //Checks if question's IsSelect property is true
                else if (questionToAdd.Question.IsSelect)
                {
                    //Creates TestQuestion Object
                    var testQuestionObj = new TestQuestion();
                    testQuestionObj.QuestionId = questionToAdd.Question.Id;
                    testQuestionObj.TestId = testId;
                    //If question is already present in the same Test, it wont be added to TestQuestion 
                    if (questionExistInTest == null)
                        testQuestionList.Add(testQuestionObj);
                }
            }
            //Returns message that user has not selected any new question 
            if (!testQuestionList.Any() && !isDeleted)
                return _stringConstants.NoNewChanges;
            else
            {
                await _dbContext.TestQuestion.AddRangeAsync(testQuestionList);
                await _dbContext.SaveChangesAsync();
                //Returns success message 
                return _stringConstants.SuccessfullySaved;
            }
        }

        public async Task<TestAC> GetTestByIdAsync(int testId)
        {
            //Find the test by Id from Test Model
            var test = await _dbContext.Test.FindAsync(testId);
            //Maps that test with TestAC
            var testAcObject = Mapper.Map<Test, TestAC>(test);
            string currentDate = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            DateTime date = DateTime.ParseExact(currentDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            if (testAcObject != null)
            {
                testAcObject.StartDate = testAcObject.StartDate == default(DateTime) ? date : testAcObject.StartDate; //If the StartDate field in database contains default value on visiting the Test Settings page of a Test for the first time then that default value gets replaced by current DateTime
                testAcObject.EndDate = testAcObject.EndDate == default(DateTime) ? date : testAcObject.EndDate; //If the EndDate field in database contains default value on visiting the Test Settings page of a Test for the first time then that default value gets replaced by current DateTime

                //Fetches the category list from Category Model
                var categoryList = await _dbContext.Category.ToListAsync();
                //Maps Category list to CategoryAC lis
                var categoryListAc = Mapper.Map<List<Category>, List<CategoryAC>>(categoryList);
                //Fetches the list of Categories from TestCategory Model
                var testCategoryList = await _dbContext.TestCategory.Where(x => x.TestId == testId).Include(x => x.Category).ToListAsync();
                categoryListAc.ForEach(category =>
                {
                    //If category present in TestCategory Model,then its IsSelect property made true
                    if (testCategoryList.Exists(x => x.CategoryId == category.Id))
                        category.IsSelect = true;
                });
                testAcObject.CategoryAcList = categoryListAc;
                return testAcObject;
            }
            else
                return null;
        }
        #endregion
    }
}