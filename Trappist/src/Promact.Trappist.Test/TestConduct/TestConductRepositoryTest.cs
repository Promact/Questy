using Promact.Trappist.Repository.TestConduct;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.Repository.Tests;
using Moq;
using Promact.Trappist.Utility.GlobalUtil;
using System.Linq;
using Promact.Trappist.Utility.Constants;

namespace Promact.Trappist.Test.TestConduct
{
    [Collection("Register Dependency")]
    public class TestConductRepositoryTest : BaseTest
    {
        #region Private Variables
        #region Dependencies
        private readonly ITestConductRepository _testConductRepository;
        private readonly ITestsRepository _testRepository;
        private readonly Mock<IGlobalUtil> _globalUtil;
        private readonly IStringConstants _stringConstants;
        #endregion
        #endregion

        #region Constructor
        public TestConductRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _testConductRepository = _scope.ServiceProvider.GetService<ITestConductRepository>();
            _testRepository = _scope.ServiceProvider.GetService<ITestsRepository>();
            _globalUtil = _scope.ServiceProvider.GetService<Mock<IGlobalUtil>>();
            _stringConstants = _scope.ServiceProvider.GetService<IStringConstants>();
        }
        #endregion

        #region Testing Methods
        /// <summary>
        /// This test case used to check test attendee successfully register.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ValidRegisterTestAttendeesAsync()
        {
            var testAttendee = InitializeTestAttendeeParameters();
            await CreateTestAsync();
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, _stringConstants.MagicString);
            Assert.True(_trappistDbContext.TestAttendees.Count() == 1);
        }

        /// <summary>
        /// This test case used to check test attendee successfully not register.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task InvalidRegisterTestAttendeesAsync()
        {
            var testAttendee = InitializeTestAttendeeParameters();
            await CreateTestAsync();
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, _stringConstants.MagicString);
            var result = await _testConductRepository.IsTestAttendeeExistAsync(testAttendee, _stringConstants.MagicString);
            Assert.True(result);
        }

        /// <summary>
        /// This test case used for check test attendee already exist.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task IsTestAttendeeExist()
        {
            var testAttendee = InitializeTestAttendeeParameters();
            await CreateTestAsync();
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendee, _stringConstants.MagicString);
            var result = await _testConductRepository.IsTestAttendeeExistAsync(testAttendee, _stringConstants.MagicString);
            Assert.True(result);
        }

        /// <summary>
        /// This test case used for check test attendee not exist.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task IsTestAttendeeNotExist()
        {
            var testAttendee = InitializeTestAttendeeParameters();
            await CreateTestAsync();
            var result = await _testConductRepository.IsTestAttendeeExistAsync(testAttendee, _stringConstants.MagicString);
            Assert.False(result);
        }

        /// <summary>
        /// This test case used to check test magic string is exist.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task IsTestLinkExistAsync()
        {
            await CreateTestAsync();
            var result = await _testConductRepository.IsTestLinkExistAsync(_stringConstants.MagicString);
            Assert.True(result);
        }

        /// <summary>
        /// This test case used to check test magic string is not exist.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task IsTestLinkNotExistAsync()
        {
            var result = await _testConductRepository.IsTestLinkExistAsync(_stringConstants.MagicString);
            Assert.False(result);
        }

        /// <summary>
        /// This method used for creating a test.
        /// </summary>
        /// <returns></returns>
        private async Task CreateTestAsync()
        {
            var test = new DomainModel.Models.Test.Test()
            {
                TestName = "GK"
            };
            _globalUtil.Setup(x => x.GenerateRandomString(10)).Returns(_stringConstants.MagicString);
            await _testRepository.CreateTestAsync(test);
        }

        /// <summary>
        ///This method used to initialize test attendee model parameters.  
        /// </summary>
        /// <returns>It return TestAttendee model object which contain first name,last name,email,contact number,roll number</returns>
        private TestAttendees InitializeTestAttendeeParameters()
        {
            var testAttendee = new TestAttendees()
            {
                FirstName = "Hardik",
                LastName = "Patel",
                Email = "phardi@gmail.com",
                ContactNumber = "1234567890",
                RollNumber = "13it055"
            };
            return testAttendee;
        }
        #endregion
    }
}