using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Test;
using System;
using System.Linq;


namespace Promact.Trappist.Repository.TestSettings
{
    public class TestSettingsRepository : ITestSettingsRepository
    {
        private readonly TrappistDbContext _dbContext;
        

        public TestSettingsRepository(TrappistDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Updates the changes made to the Settings of a Test
        /// </summary>
        /// <param name="id">The parameter "id" is used to access the Settings of that Test</param>
        /// <param name="testObject">The parameter "testObject" is used as an object of the Model Test</param>
        /// <returns>Updated Setting of that Test</returns>
        public string UpdateTestSettings(int id, Test testObject)
        {
            var settingsToUpdate = _dbContext.Test.FirstOrDefault(check => check.Id == id);
            settingsToUpdate.TestName = testObject.TestName;
            settingsToUpdate.BrowserTolerance = testObject.BrowserTolerance;
            settingsToUpdate.StartDate = testObject.StartDate;
            settingsToUpdate.EndDate = testObject.EndDate;
            settingsToUpdate.Duration = testObject.Duration;
            settingsToUpdate.FromIpAddress = testObject.FromIpAddress;
            settingsToUpdate.ToIpAddress = testObject.ToIpAddress;
            settingsToUpdate.WarningMessage = testObject.WarningMessage;
            settingsToUpdate.CorrectMarks = testObject.CorrectMarks;
            settingsToUpdate.IncorrectMarks = testObject.IncorrectMarks;
            settingsToUpdate.WarningTime = testObject.WarningTime;
            _dbContext.Test.Update(settingsToUpdate);
            _dbContext.SaveChanges();
            return "Data Updated";
        }

        /// <summary>
        /// Gets the Settings saved for a particular Test
        /// </summary>
        /// <param name="id">The parameter "id" is used to get the Settings of a Test by its Id</param>
        /// <returns>Settings Saved for the selected Test</returns>
        public Test GetTestSettings(int id)
        {
            Test test = new Test();
            DateTime date = DateTime.Now;
            string currentDate = date.ToString("MM/dd/yyyy HH:mm");
            test.StartDate = Convert.ToDateTime(currentDate);
            test.EndDate = Convert.ToDateTime(currentDate);
            test.BrowserTolerance = 0;
            var testSettings = _dbContext.Test.FirstOrDefault(x => x.Id == id);
            return testSettings;
        }
    }
}
