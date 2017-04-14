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
        /// Test Case for fetching settings of a test from database with the help of Id
        /// </summary>
        [Fact]
        public async Task GetSettingsById()
        {
            var test = CreateTest("AOT 669");
            await _testRepository.CreateTestAsync(test);
            Assert.NotNull(test);
            var testSettings = await _testRepository.GetTestByIdAsync(test.Id);
            var testName = testSettings.TestName;
            Assert.Equal(testName, "AOT 669");
        }

        /// <summary>
        /// Test Case for updating the settings set for a test in the database with the help of Id
        /// </summary>
        [Fact]
        public async Task UpdateTestSettingsById()
        {
            var test = CreateTest("AOT 669");
            await _testRepository.CreateTestAsync(test);
            var testSettingsToUpdate = await _testRepository.GetTestByIdAsync(test.Id);
            testSettingsToUpdate.TestName = "IIT BANGALORE";
            testSettingsToUpdate.BrowserTolerance = 2;
            await _testRepository.UpdateTestByIdAsync(testSettingsToUpdate);
            var TestName = "IIT BANGALORE";
            Assert.True(_trappistDbContext.Test.Count(x => x.TestName == TestName) == 1);
            Assert.True(_trappistDbContext.Test.Count(x => x.BrowserTolerance == 2) == 1);
        }

        /// <summary>
        /// Test Case for updating the edited test name in the database 
        /// </summary>
        [Fact]
        public async Task UpdateTestName()
        {
            var test = CreateTest("AOT 669");
            await _testRepository.CreateTestAsync(test);
            var testNameToUpdate = await _testRepository.GetTestByIdAsync(test.Id);
            testNameToUpdate.TestName = "MCKV";
            await _testRepository.UpdateTestNameAsync(test.Id, testNameToUpdate);
            var TestName = "MCKV";
            Assert.True(_trappistDbContext.Test.Count(x => x.TestName == TestName) == 1);
        }

        /// <summary>
        /// Check if any test attendee exist for a particular test
        /// </summary>
        [Fact]
        public async Task IsAttendeeExistAsync()
        {
            var test = CreateTest("Aptitude");
            var testAttendee = TestAttendee();
            await _testRepository.CreateTestAsync(test);
            testAttendee.Test = test;
            testAttendee.TestId = test.Id;
            _trappistDbContext.TestAttendees.Add(testAttendee);
            var isExists = await _testRepository.IsTestAttendeeExistAsync(test.Id);
            Assert.True(isExists);
        }

        /// <summary>
        /// CHeck that no Test attendee exist for a particular test 
        /// </summary>
        [Fact]
        public async Task IsAttendeeNotExistAsync()
        {
            var test = CreateTest("Aptitude");
            await _testRepository.CreateTestAsync(test);
            var isNotExist = await _testRepository.IsTestAttendeeExistAsync(test.Id);
            Assert.False(isNotExist);
        }

        /// <summary>
        /// Delete the selected test
        /// </summary>
        [Fact]
        public async Task DeleteTestAsync()
        {
            var test = CreateTest("Logical");
            await _testRepository.CreateTestAsync(test);
            await _testRepository.DeleteTestAsync(test.Id);
            Assert.Equal(0, _trappistDbContext.Test.Count());
        }

        private DomainModel.Models.Test.Test CreateTest(string testName)
        {
            var test = new DomainModel.Models.Test.Test
            {
                TestName = testName,
                BrowserTolerance = 1
            };
            return test;
        }

        private DomainModel.Models.TestConduct.TestAttendees TestAttendee()
        {
            var testAttendee = new DomainModel.Models.TestConduct.TestAttendees()
            {
                FirstName = "Ritika",
                LastName = "Mohata",
                Email = "ritika@gmail.com"
            };
            return testAttendee;
        }
    }
}