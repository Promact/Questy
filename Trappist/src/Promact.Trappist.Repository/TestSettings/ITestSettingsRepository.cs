using Promact.Trappist.DomainModel.Models.Test;
using System.Collections.Generic;


namespace Promact.Trappist.Repository.TestSettings
{
     public interface ITestSettingsRepository
    {
        /// <summary>
        /// Get the Settings saved for a particular Test
        /// </summary>
        /// <returns>Settings set for that Test</returns>
        List<Test> GetTestSettings();

        /// <summary>
        /// Updates the changes made to the Settings of a Test
        /// </summary>
        /// <param name="id">The parameter "id" is used to access the Settings of that Test</param>
        /// <param name="testObject">The parameter "testObject" is used as an object of the Model Test</param>
        /// <returns>Updated Settings of that Test</returns>
        string UpdateTestSettings(int id, Test testObject);
    }
}
