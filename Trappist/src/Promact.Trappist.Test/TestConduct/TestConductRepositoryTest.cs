using Promact.Trappist.Repository.TestConduct;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.Repository.Tests;
using Moq;
using Promact.Trappist.Utility.GlobalUtil;

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
        #endregion
        #endregion

        #region Constructor
        public TestConductRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _testConductRepository = _scope.ServiceProvider.GetService<ITestConductRepository>();
            _testRepository = _scope.ServiceProvider.GetService<ITestsRepository>();
            _globalUtil = _scope.ServiceProvider.GetService<Mock<IGlobalUtil>>();
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
            var model = InitializeTestAttendeeParameters();
            await CreateTestAsync();
            var result = await _testConductRepository.RegisterTestAttendeesAsync(model, "H0SGXPOwsR");
            Assert.True(result);
        }

        /// <summary>
        /// This test case used to check test attendee already exist.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task InvalidRegisterTestAttendeesAsync()
        {
            var model = InitializeTestAttendeeParameters();
            await CreateTestAsync();
            await _testConductRepository.RegisterTestAttendeesAsync(model, "H0SGXPOwsR");
            var result = await _testConductRepository.RegisterTestAttendeesAsync(model, "H0SGXPOwsR");
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
            _globalUtil.Setup(x => x.GenerateRandomString(10)).Returns("H0SGXPOwsR");
            await _testRepository.CreateTestAsync(test);
        }

        /// <summary>
        ///This method used to initialize test attendee model parameters.  
        /// </summary>
        /// <returns>It return TestAttendee model object which contain first name,last name,email,contact number,roll number</returns>
        private TestAttendees InitializeTestAttendeeParameters()
        {
            var model = new TestAttendees()
            {
                FirstName = "Hardik",
                LastName = "Patel",
                Email = "phardik026@gmail.com",
                ContactNumber = "8469200465",
                RollNumber = "13it055"
            };
            return model;
        }
        #endregion
    }
}