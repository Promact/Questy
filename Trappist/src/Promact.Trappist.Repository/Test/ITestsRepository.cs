using Promact.Trappist.DomainModel.Models.Test;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Promact.Trappist.Repository.Tests
{
    public interface ITestsRepository
    {
        /// <summary>
        /// this method is used to create a new test
        /// </summary>
        /// <param name="test">object of Test</param>
        Task CreateTestAsync(Test test);

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
        Task<List<Test>> GetAllTestsAsync();

        /// <summary>
        /// Gets the Settings saved for a particular Test
        /// </summary>
        /// <param name="id">The parameter "id" is used to get the Settings of a Test by its Id</param>
        /// <returns>Settings Saved for the selected Test</returns>
        Task<Test> GetTestByIdAsync(int id);

        /// <summary>
        /// Updates the changes made to the settings of a Test
        /// </summary>
        /// <param name="testObject">The parameter "testObject" is an object of Test</param>
        /// <returns>Updated Settings of that Test</returns>
        Task UpdateTestByIdAsync(Test testObject);

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
        Task UpdateTestNameAsync(int id, Test testObject);

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
    }
}