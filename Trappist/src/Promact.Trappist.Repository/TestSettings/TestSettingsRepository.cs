using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.ApplicationClasses.TestSettings;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Test;
using System;
using System.Threading.Tasks;

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
        /// Updates the changes made to the settings of a Test
        /// </summary>
        /// <param name="testACObject">The parameter "testACObject" is an object of TestSettingsAC</param>
        /// <returns>Updated Settings of that Test</returns>
        public async Task UpdateTestSettingsAsync(TestSettingsAC testACObject)
        {
            Test settings = Mapper.Map<TestSettingsAC, Test>(testACObject);
            _dbContext.Test.Update(settings);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the Settings saved for a particular Test
        /// </summary>
        /// <param name="id">The parameter "id" is used to get the Settings of a Test by its Id</param>
        /// <returns>Settings Saved for the selected Test</returns>
        public async Task<Test> GetTestSettingsAsync(int id)
        {
            string currentDate = DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm");
            var testSettings = await _dbContext.Test.FirstOrDefaultAsync(x => x.Id == id);
            if (testSettings != null)
            {
                testSettings.StartDate = testSettings.StartDate == default(DateTime) ? Convert.ToDateTime(currentDate) : testSettings.StartDate; //If the StartDate field in database contains default value on visiting the Test Settings page of a Test for the first time then that default value gets replaced by current DateTime
                testSettings.EndDate = testSettings.EndDate == default(DateTime) ? Convert.ToDateTime(currentDate) : testSettings.EndDate; //If the EndDate field in database contains default value on visiting the Test Settings page of a Test for the first time then that default value gets replaced by current DateTime
                return testSettings;
            }
            else
                return null;
        }

        /// <summary>
        /// Checks if the Test Settings Exists or not
        /// </summary>
        /// <param name="id">The parameter "id" is taken from the route</param>
        /// <returns>A bool value based on the condition is satisfied or not</returns>
        public async Task<bool> TestSettingsExists(int id)
        {
            return await _dbContext.Test.AnyAsync(check => check.Id != id);
        }
    }
}
