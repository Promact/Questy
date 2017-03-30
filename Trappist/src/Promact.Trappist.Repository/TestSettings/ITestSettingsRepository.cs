using Promact.Trappist.DomainModel.ApplicationClasses.TestSettings;
using Promact.Trappist.DomainModel.Models.Test;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.TestSettings
{
    public interface ITestSettingsRepository
    {
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
        Task<bool> TestSettingsExists(int id);
    }
}
