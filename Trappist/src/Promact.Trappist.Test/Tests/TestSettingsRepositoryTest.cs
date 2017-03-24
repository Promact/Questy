using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.Repository.TestSettings;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Promact.Trappist.Repository.Tests;
using Xunit;

namespace Promact.Trappist.Test.Tests
{
    [Collection("Register Dependency")]
    public class TestSettingsRepositoryTest
    {
        private readonly Bootstrap _bootstrap;
        private readonly TrappistDbContext _trappistDbContext;
        private readonly ITestSettingsRepository _settingsRepository;
        private readonly ITestsRepository _testRepository;

        public TestSettingsRepositoryTest(Bootstrap bootstrap)
        {
            _bootstrap = bootstrap;
            //resolve dependencies to be used in tests
            _trappistDbContext = _bootstrap.ServiceProvider.GetService<TrappistDbContext>();
            _settingsRepository = _bootstrap.ServiceProvider.GetService<ITestSettingsRepository>();
            _testRepository = _bootstrap.ServiceProvider.GetService<ITestsRepository>();
            ClearDatabase.ClearDatabaseAndSeed(_trappistDbContext);
        }

        /// <summary>
        /// Gets Settings of Test selected by Id
        /// </summary>
        [Fact]
        public void GetSettingsById()
        {
            var test = AddNewTest();
            _testRepository.CreateTest(test);
            Assert.NotNull(test);
            var testSettings = _settingsRepository.GetTestSettings(test.Id);
            var testName = testSettings.TestName;
            Assert.Equal(testName, "AOT 669");
        }

        /// <summary>
        /// Updates settings of a particular test with the help of Id
        /// </summary>
        [Fact]
        public void UpdateTestSettingsById()
        {
            var test = AddNewTest();
            _testRepository.CreateTest(test);
            var settingsToUpdate = _settingsRepository.GetTestSettings(test.Id);
            settingsToUpdate.TestName = "IIT BANGALORE";
            _trappistDbContext.Test.Update(settingsToUpdate);
            _trappistDbContext.SaveChanges();
            Assert.True(_trappistDbContext.Test.Count(x => x.TestName == "IIT BANGALORE") == 1);
        }

        /// <summary>
        /// Adds new Test in the database
        /// </summary>
        /// <returns>The data added in the database</returns>
        public DomainModel.Models.Test.Test AddNewTest()
        {
            var test = new DomainModel.Models.Test.Test()
            {
                TestName = "AOT 669"
            };
            return test;
        }
    }
}
