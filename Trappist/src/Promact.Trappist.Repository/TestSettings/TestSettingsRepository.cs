using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Test;
using System.Collections.Generic;
using System.Linq;


namespace Promact.Trappist.Repository.TestSettings
{
    public class TestSettingsRepository : ITestSettingsRepository
    {
        private readonly TrappistDbContext _dbContext;
        Test test = new Test();

        public TestSettingsRepository(TrappistDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Updates the changes made to the Settings of a Test
        /// </summary>
        /// <param name="id"></param>
        /// <param name="testObject"></param>
        /// <returns></returns>
        public string UpdateTestSettings(int id, Test testObject)
        {
            testObject = new Test();
            test = (from s in _dbContext.Test where s.Id == id select s).FirstOrDefault();
            test = testObject;
            _dbContext.Test.Update(test);
            _dbContext.SaveChanges();
            return "Data Updated";
        }

        /// <summary>
        /// Get the Settings saved for a particular Test
        /// </summary>
        /// <returns>Settings set for that Test</returns>
        public List<Test> GetTestSettings()
        {
            var settings = _dbContext.Test.ToList();
            return settings;
        }
    }
}
