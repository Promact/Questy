using Microsoft.AspNetCore.Identity;
using Moq;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Report;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.Repository.Reports;
using Promact.Trappist.Repository.TestConduct;
using Promact.Trappist.Repository.Tests;
using Promact.Trappist.Utility.Constants;
using Promact.Trappist.Utility.GlobalUtil;
using Promact.Trappist.Web.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace Promact.Trappist.Test.Reports
{
    [Collection("Register Dependency")]
    public class ReportsRepositoryTest : BaseTest
    {
        private readonly ITestConductRepository _testConductRepository;
        private readonly IReportRepository _reportRepository;
        private readonly Mock<IGlobalUtil> _globalUtil;
        private readonly IStringConstants _stringConstants;
        private readonly ITestsRepository _testRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _testConductRepository = _scope.ServiceProvider.GetService<ITestConductRepository>();
            _reportRepository = _scope.ServiceProvider.GetService<IReportRepository>();
            _globalUtil = _scope.ServiceProvider.GetService<Mock<IGlobalUtil>>();
            _stringConstants = _scope.ServiceProvider.GetService<IStringConstants>();
            _testRepository = _scope.ServiceProvider.GetService<ITestsRepository>();
            _userManager = _scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
        }

        /// <summary>
        /// Test to get test attendee report
        /// </summary>
        [Fact]
        public async Task GetTestAttendeeReportAsyncTest()
        {
            var createTest = await CreateTestAsync();
            var testAttendeeReport = TestAttendeeReport(createTest.Id);
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendeeReport, _stringConstants.MagicString);
            var report = await _reportRepository.GetAllTestAttendeesAsync(createTest.Id);
            Assert.True(report.Count() == 1);
        }

        /// <summary>
        /// Test to set a candidate as starred candidate
        /// </summary>
        [Fact]
        public async Task SetStarredCandidateAsyncTest()
        {
            var createTest = await CreateTestAsync();
            var testAttendeeReport = TestAttendeeReport(createTest.Id);
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendeeReport, _stringConstants.MagicString);
            await _reportRepository.SetStarredCandidateAsync(testAttendeeReport.Id);
            Assert.True(testAttendeeReport.StarredCandidate.Equals(false));
        }

        /// <summary>
        /// Test to set all candidates matching certain criterias as starred candidate
        /// </summary>
        [Fact]
        public async Task SetAllCandidateStarredAsync()
        {
            var createTest = await CreateTestAsync();
            var testAttendeeReport = TestAttendeeReport(createTest.Id);
            await _testConductRepository.RegisterTestAttendeesAsync(testAttendeeReport, _stringConstants.MagicString);
            await _reportRepository.SetAllCandidateStarredAsync(false, 3, "");
            Assert.True(testAttendeeReport.StarredCandidate.Equals(false));
        }

        /// <summary>
        ///This method used to initialize test attendee model parameters.  
        /// </summary>
        /// <returns>It return TestAttendee model object which contain first name,last name,email,contact number,roll number</returns>
        private TestAttendees TestAttendeeReport(int id)
        {
            var testAttendee = new TestAttendees()
            {
                Id = 1,
                FirstName = "Hardik",
                LastName = "Patel",
                Email = "phardi@gmail.com",
                ContactNumber = "1234567890",
                RollNumber = "13it055",
                StarredCandidate = true,
                TestId = id,
                Report = new Report()
                {
                    Percentage = 45.55,
                    Percentile = 46,
                    TotalMarksScored = 45.55,
                    TestStatus = TestStatus.BlockedTest,
                    TimeTakenByAttendee = 45
                }
            };
            return testAttendee;
        }

        /// <summary>
        /// This method used for creating a test.
        /// </summary>
        /// <returns></returns>
        private async Task<DomainModel.Models.Test.Test> CreateTestAsync()
        {
            var test = new DomainModel.Models.Test.Test()
            {
                TestName = "GK",
                Duration = 70,
                WarningTime = 2,
                CorrectMarks = 4,
                IncorrectMarks = -1,
                BrowserTolerance = 1

            };
            _globalUtil.Setup(x => x.GenerateRandomString(10)).Returns(_stringConstants.MagicString);
            string userName = "suparna@promactinfo.com";
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName };
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _testRepository.CreateTestAsync(test, applicationUser.Id);
            return test;
        }
    }
}