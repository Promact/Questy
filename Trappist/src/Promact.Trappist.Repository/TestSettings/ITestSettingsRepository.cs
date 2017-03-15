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
        /// <param name="id"></param>
        /// <param name="test1"></param>
        /// <returns>Updated Settings of that Test</returns>
        string UpdateTestSettings(int id, Test test1);
    }
}
