﻿
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Promact.Trappist.Repository.Tests;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.Test.Tests
{
    [Collection("Register Dependency")]
    public class TestsRepositoryTest : BaseTest
    {
        private readonly ITestsRepository _testRepository;

        public TestsRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _testRepository = _scope.ServiceProvider.GetService<ITestsRepository>();
        }

        /// <summary>
        /// Test Case For Emtpty Test Model
        /// </summary>
        [Fact]
        public async Task GetAllTestEmpty()
        {
            var list = await _testRepository.GetAllTestsAsync();
            Assert.Equal(0, list.Count);
        }

        /// <summary>
        /// Test Case for adding a new test
        /// </summary>
        [Fact]
        public async Task AddTest()
        {
            var test = CreateTest("testname");
            await _testRepository.CreateTestAsync(test);
            Assert.True(_trappistDbContext.Test.Count() == 1);
        }

        /// <summary>
        /// Test  Case to create a new test when the test name given is unique
        /// </summary>
        [Fact]
        public async Task UniqueNameTest()
        {
            var test = CreateTest("testname");
            await _testRepository.CreateTestAsync(test);
            var name = "nameOfTest";
            var id = 0;
            bool isExist = await _testRepository.IsTestNameUniqueAsync(name, id);
            Assert.True(isExist);
        }

        /// <summary>
        /// Test Case to check when test name is not unique, new test is not added .
        /// </summary>
        [Fact]
        public async Task IsNotUniqueNameTest()
        {
            var test = CreateTest("test name");
            await _testRepository.CreateTestAsync(test);
            var name = "Test name";
            var id = 0;
            bool isExist = await _testRepository.IsTestNameUniqueAsync(name, id);
            Assert.False(isExist);
        }

        /// <summary>
        /// Gets Settings of Test selected by Id
        /// </summary>
        [Fact]
        public async Task GetSettingsById()
        {
            var test = CreateTest("AOT 669");
            await _testRepository.CreateTestAsync(test);
            Assert.NotNull(test);
            var testSettings = await _testRepository.GetTestSettingsAsync(test.Id);
            var testName = testSettings.TestName;
            Assert.Equal(testName, "AOT 669");
        }

        /// <summary>
        /// Updates settings of a particular test with the help of Id
        /// </summary>
        [Fact]
        public async Task UpdateTestSettingsById()
        {
            var test = CreateTest("AOT 669");
            await _testRepository.CreateTestAsync(test);
            var settingsToUpdate = await _testRepository.GetTestSettingsAsync(test.Id);
            settingsToUpdate.TestName = "IIT BANGALORE";
            await _testRepository.UpdateTestSettingsAsync(settingsToUpdate);
            var TestName = "IIT BANGALORE";
            await _trappistDbContext.SaveChangesAsync();
            Assert.True(_trappistDbContext.Test.Count(x => x.TestName == TestName) == 1);
        }

        private DomainModel.Models.Test.Test CreateTest(string testName)
        {
            var test = new DomainModel.Models.Test.Test
            {
                TestName = testName
            };
            return test;
        }
    }
}