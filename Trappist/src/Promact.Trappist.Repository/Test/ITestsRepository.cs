using Promact.Trappist.DomainModel.Models.Test;
using System.Threading.Tasks;
using System.Collections.Generic;
using Promact.Trappist.DomainModel.ApplicationClasses.TestSettings;

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
        Task<Test> GetTestSettingsAsync(int id);

        /// <summary>
        /// Updates the changes made to the settings of a Test
        /// </summary>
        /// <param name="testACObject">The parameter "testACObject" is an object of TestSettingsAC</param>
        /// <returns>Updated Settings of that Test</returns>
        Task UpdateTestSettingsAsync(TestSettingsAC testACObject);

        /// <summary>
        /// Checks if the Test Settings Exists or not
        /// </summary>
        /// <param name="id">The parameter "id" is taken from the route</param>
        /// <returns>A bool value based on the condition is satisfied or not</returns>
        Task<bool> IsTestExists(int id);
    }
}