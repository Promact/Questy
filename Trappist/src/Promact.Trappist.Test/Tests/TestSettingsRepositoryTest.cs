using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.Repository.TestSettings;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace Promact.Trappist.Test.Tests
{
    [Collection("Register Dependency")]
    public class TestSettingsRepositoryTest
    {
        private readonly Bootstrap _bootstrap;
        private readonly TrappistDbContext _trappistDbContext;
        private readonly ITestSettingsRepository _settingsRepository;

        public TestSettingsRepositoryTest(Bootstrap bootstrap)
        {
            _bootstrap = bootstrap;
            //resolve dependencies to be used in tests
            _trappistDbContext = _bootstrap.ServiceProvider.GetService<TrappistDbContext>();
            _settingsRepository = _bootstrap.ServiceProvider.GetService<ITestSettingsRepository>();
            ClearDatabase.ClearDatabaseAndSeed(_trappistDbContext);
        }

        private void AddNewTest()
        {
            _trappistDbContext.Test.Add(new DomainModel.Models.Test.Test() { TestName = "AOT 669" });
            _trappistDbContext.Test.Add(new DomainModel.Models.Test.Test() { TestName = "MCKVIE 142" });
            _trappistDbContext.Test.Add(new DomainModel.Models.Test.Test() { TestName = "IEM 336" });
            _trappistDbContext.SaveChanges();
        }

        [Fact]
        public void GetTestSettingsById()
        {
            AddNewTest();
            var testSettings = _settingsRepository.GetTestSettings(1);
            var testName = testSettings.TestName;
            Assert.Equal(testName, "AOT 669");
        }

        [Fact]
        public void UpdateTestSettingsById()
        {
            AddNewTest();
            var settingsToUpdate = _settingsRepository.GetTestSettings(2);
            settingsToUpdate.TestName = "IIT BANGALORE";
            _settingsRepository.UpdateTestSettings(2, settingsToUpdate);
            Assert.True(_trappistDbContext.Test.Count(x => x.TestName == "IIT BANGALORE") == 1);
        }
    }
}
