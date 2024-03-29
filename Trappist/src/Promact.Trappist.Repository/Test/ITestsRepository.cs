﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.ApplicationClasses.Test;
using Promact.Trappist.DomainModel.ApplicationClasses.TestConduct;
using Promact.Trappist.DomainModel.Models.Test;

namespace Promact.Trappist.Repository.Test
{
    public interface ITestsRepository
    {
        #region Test
        /// <summary>
        /// this method is used to create a new test
        /// </summary>
        /// <param name="test">object of Test</param>
        /// <param name="userId">id of the user who created the test</param>
        /// <returns></returns>
        Task CreateTestAsync(DomainModel.Models.Test.Test test, string userId);

        /// <summary>
        /// this method is used to verify Name of the test is unique or not
        /// </summary>
        /// <param name="testName">name of the test</param>
        /// <returns>boolean</returns>
        Task<bool> IsTestNameUniqueAsync(string testName, int id);

        /// <summary>
        /// Method defined to fetch Tests from Test Model
        /// </summary>
        /// <returns>List Of Tests</returns>
        Task<List<TestAC>> GetAllTestsAsync();

        /// <summary>
        /// Sets the start test time log of the test attendee
        /// </summary>
        /// <param name="attendeeId">Id of the test attendee</param>
        /// <returns></returns>
        Task SetStartTestLogAsync(int attendeeId);
        #endregion

        #region TestSettings

        /// <summary>
        /// Updates the changes made to the settings of a Test
        /// </summary>
        /// <param name="testObject">The parameter "testObject" is an object of Test</param>
        /// <returns>Updated Settings of that Test</returns>
        Task UpdateTestByIdAsync(DomainModel.Models.Test.Test testObject);

        /// <summary>
        /// Checks if the Test Settings Exists or not
        /// </summary>
        /// <param name="id">The parameter "id" is taken from the route</param>
        /// <returns>A bool value based on the condition is satisfied or not</returns>
        Task<bool> IsTestExists(int id);

        /// <summary>
        /// Updates the edited Test Name
        /// </summary>
        /// <param name="id">The parameter "id" takes the value of the Id from the route</param>
        /// <param name="testObject">The parameter "testObject" is an object of Test</param>
        /// <returns>Updated Test Name</returns>
        Task UpdateTestNameAsync(int id, DomainModel.Models.Test.Test testObject);

        #endregion

        #region Test-Pause-Resume

        /// <summary>
        /// Pauses the launched Test and resumes the pauesd test
        /// </summary>
        /// <param name="id">id is test Id</param>
        /// <param name="isPause">isPaused is boolean Test property</param>
        /// <returns></returns>
        Task PauseResumeTestAsync(int id, bool isPause);

        /// <summary>
        /// Checks if Test can be edited or not
        /// </summary>
        /// <param name="id">id is test Id</param>        
        /// <returns>Returns true if Test is editable else false</returns>
        Task<bool> IsTestEditableAsync(int id);
        #endregion

        #region Delete-Test-Ip-Address
        /// <summary>
        /// Deletes Ip Address field for a test
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteIpAddressAsync(int id);
        #endregion

        #region Delete Test
        /// <summary>
        /// Delete test from the test model
        /// </summary>
        ///<param name="id">Id of the test to be deleted</param>
        /// <returns>Delete the test from database and save the changes</returns>
        Task DeleteTestAsync(int id);

        /// <summary>
        /// Check whether a test attendee exist or not
        /// </summary>
        /// <param name="id">Id of the test</param>
        /// <returns>Boolean:true if an attendee exist or else false</returns>
        Task<bool> IsTestAttendeeExistAsync(int id);
        #endregion

        Task<DomainModel.Models.Test.Test> GetTestSummary(string link);

        #region Category Selection       

        /// <summary>
        /// To add a category to the test when it is selected
        /// </summary>
        /// <param name="testId"
        /// <param name="categoryAcList"></param>
        /// <returns>list of categories added</returns>        
        Task AddTestCategoriesAsync(int testId, List<TestCategoryAC> categoryAcList);

        /// <summary>
        /// To deselect a category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="testId"></param>
        /// <returns>boolean</returns>
        Task<bool> DeselectCategoryAync(int categoryId, int testId);

        /// <summary>
        /// To delete the deselected category from TestCategory model
        /// </summary>
        /// <param name="testCategory"></param>
        /// <returns>Deletes the category from TestCategory model and also deletes if any question from that category exists in the test and save changes to the database</returns>
        Task RemoveCategoryAndQuestionAsync(TestCategory testCategory);
        #endregion

        #region Test-Question-Selection
        /// <summary>
        /// Gets all the questions contained by a particular category in a test
        /// </summary>
        /// <param name="testId">Parameter "testId" takes value of a Test's Id from route </param>
        /// <param name="CategoryId">Parameter "CategoryId" takes value of a Category's Id from route</param>
        /// <param name="userId">User's Id with which user has logged in</param>
        /// <returns>List of QuestionAC objects of particular category</returns>      
        Task<List<QuestionAC>> GetAllQuestionsByTestIdAsync(int testId, int categoryId, string userId);

        /// <summary>
        /// Adds selected question(s) to the particular "Test" 
        /// </summary>
        /// <param name="QuestionsToAddTest">Parameter "QuestionsToAddTest" is a list of questions to be added to Test which takes value from body</param>
        /// <param name="testId">Parameter "testId" takes value of a Test's Id from route</param>
        /// <returns>A string message of Success</returns>
        Task<string> AddTestQuestionsAsync(List<TestQuestionAC> questionsToAdd, int testId);

        /// <summary>
        /// Get details of a Test
        /// </summary>
        /// <param name="testId">Parameter "testId" takes value of a Test's Id from route</param>
        /// <param name="userId">User's Id with which user has logged in</param>
        /// <returns>TestAC object</returns>       
        Task<TestAC> GetTestByIdAsync(int testId, string userId);

        /// <summary>
        /// Gets all the question by Test Id. No answers are returned with the question.
        /// </summary>
        /// <param name="testId">Id of Test</param>
        /// <returns>Collection of QuestionAC object</returns>
        Task<ICollection<TestConductAC>> GetTestQuestionByTestIdAsync(int testId);

        /// <summary>
        /// Gets test by link  
        /// </summary>
        /// <param name="link">Link of the test</param>
        /// <returns>TestAC object</returns>
        Task<TestAC> GetTestByLinkAsync(string link);
        #endregion

        #region Duplicate Test
        /// <summary>
        /// Duplicates questions,categories and Ip addresses present in the test to be duplicated
        /// </summary>
        /// <param name="testId">Id of the test that is to be duplicated</param>
        /// <param name="newtestId">Id of the duplicated Test</param>
        /// <returns>Test object for the duplicated test</returns>
        Task<DomainModel.Models.Test.Test> DuplicateTest(int testId, DomainModel.Models.Test.Test test);

        /// <summary>
        /// Sets the number of times the test has been duplicated
        /// </summary>
        /// <param name="testName">Contains the name of the test which is to be duplicated</param>
        /// <returns>The number of times the test has been duplicated</returns>
        Task<int> SetTestCopiedNumberAsync(string testName);
        #endregion
    }
}