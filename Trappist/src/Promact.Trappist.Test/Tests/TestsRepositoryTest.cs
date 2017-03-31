using Promact.Trappist.DomainModel.DbContext;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Promact.Trappist.Repository.Tests;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.Test.Tests
{
    [Collection("Register Dependency")]
    public class TestsRepositoryTest
    {
        private readonly ITestsRepository _testRepository;
        private readonly TrappistDbContext _trappistDbContext;

        public TestsRepositoryTest(Bootstrap bootstrap)
        {
            //resolve dependency to be used in tests
            _trappistDbContext = bootstrap.ServiceProvider.GetService<TrappistDbContext>();
            _testRepository = bootstrap.ServiceProvider.GetService<ITestsRepository>();
            ClearDatabase.ClearDatabaseAndSeed(_trappistDbContext);
        }
        /// <summary>
        /// Test Case For Not Empty Test Model
        /// </summary>
        [Fact]
        public async Task GetAllTest()
        {
            AddTests();
            var list = await _testRepository.GetAllTestsAsync();
            Assert.NotNull(list);
            Assert.Equal(3, list.Count);
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
            bool isExist = await _testRepository.IsTestNameUniqueAsync(name);
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
            bool isExist = await _testRepository.IsTestNameUniqueAsync(name);
            Assert.False(isExist);
        }

        public void AddTests()
        {
            _trappistDbContext.Test.Add(new DomainModel.Models.Test.Test() { TestName = "BBIT 123" });
            _trappistDbContext.Test.Add(new DomainModel.Models.Test.Test() { TestName = "MCKV 123" });
            _trappistDbContext.Test.Add(new DomainModel.Models.Test.Test() { TestName = "CU 123" });
            _trappistDbContext.SaveChanges();
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