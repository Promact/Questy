using Promact.Trappist.DomainModel.Models.Test;

namespace Promact.Trappist.Repository.TestSettings
{
     public interface ITestSettingsRepository
    {
        /// <summary>
        /// Gets the Settings saved for a particular Test
        /// </summary>
        /// <param name="id">The parameter "id" is used to get the Settings of a Test by its Id</param>
        /// <returns>Settings Saved for the selected Test</returns>
        Test GetTestSettings(int id);

        /// <summary>
        /// Updates the changes made to the Settings of a Test
        /// </summary>
        /// <param name="id">The parameter "id" is used to access the Settings of that Test</param>
        /// <param name="testObject">The parameter "testObject" is used as an object of the Model Test</param>
        /// <returns>Updated Settings of that Test</returns>
        string UpdateTestSettings(int id, Test testObject);
    }
}
