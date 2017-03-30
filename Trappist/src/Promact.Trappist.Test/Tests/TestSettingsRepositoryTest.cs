﻿using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.Repository.TestSettings;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Promact.Trappist.Repository.Tests;
using Xunit;
using System.Threading.Tasks;

namespace Promact.Trappist.Test.Tests
{
    [Collection("Register Dependency")]
    public class TestSettingsRepositoryTest
    {
        private readonly TrappistDbContext _trappistDbContext;
        private readonly ITestSettingsRepository _settingsRepository;
        private readonly ITestsRepository _testRepository;

        public TestSettingsRepositoryTest(Bootstrap bootstrap)
        {
            //resolve dependencies to be used in tests
            _trappistDbContext = bootstrap.ServiceProvider.GetService<TrappistDbContext>();
            _settingsRepository = bootstrap.ServiceProvider.GetService<ITestSettingsRepository>();
            _testRepository = bootstrap.ServiceProvider.GetService<ITestsRepository>();
            ClearDatabase.ClearDatabaseAndSeed(_trappistDbContext);
        }

        /// <summary>
        /// Gets Settings of Test selected by Id
        /// </summary>
        [Fact]
        public async Task GetSettingsById()
        {
            var test = AddNewTest();
            _testRepository.CreateTest(test);
            Assert.NotNull(test);
            var testSettings = await _settingsRepository.GetTestSettingsAsync(test.Id);
            var testName = testSettings.TestName;
            Assert.Equal(testName, "AOT 669");
        }

        /// <summary>
        /// Updates settings of a particular test with the help of Id
        /// </summary>
        [Fact]
        public async Task UpdateTestSettingsById()
        {
            var test = AddNewTest();
            _testRepository.CreateTest(test);
            var settingsToUpdate = await _settingsRepository.GetTestSettingsAsync(test.Id);
            settingsToUpdate.TestName = "IIT BANGALORE";
            _trappistDbContext.Test.Update(settingsToUpdate);
            var TestName = "IIT BANGALORE";
            await _trappistDbContext.SaveChangesAsync();
            Assert.True(_trappistDbContext.Test.Count(x => x.TestName == TestName) == 1);
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
