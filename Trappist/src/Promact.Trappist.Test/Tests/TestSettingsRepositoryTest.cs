using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.Repository.TestSettings;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;
using System.Collections.Generic;

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

        private List<DomainModel.Models.Test.Test> AddNewTest()
        {
            _trappistDbContext.Test.Add(new DomainModel.Models.Test.Test() { TestName = "AOT 669" });
            _trappistDbContext.Test.Add(new DomainModel.Models.Test.Test() { TestName = "MCKVIE 142"});
            _trappistDbContext.Test.Add(new DomainModel.Models.Test.Test() { TestName = "IEM 336"});
            _trappistDbContext.SaveChanges();

            return _trappistDbContext.Test.ToList();
                
        }

        [Fact]
        public void GetTestSettingsById()
        {
            var list = AddNewTest();
            var testSettings = _settingsRepository.GetTestSettings(list[0].Id);
            var testName = testSettings.TestName;
            Assert.Equal(testName, "AOT 669");
        }

        [Fact]
        public void UpdateTestSettingsById()
        {
            var list=AddNewTest();
            var settingsToUpdate = _settingsRepository.GetTestSettings(list[0].Id);
            settingsToUpdate.TestName = "IIT BANGALORE";
            _settingsRepository.UpdateTestSettings(list[0].Id, settingsToUpdate);
            Assert.True(_trappistDbContext.Test.Count(x => x.TestName == "IIT BANGALORE") == 1);
        }
    }
}
