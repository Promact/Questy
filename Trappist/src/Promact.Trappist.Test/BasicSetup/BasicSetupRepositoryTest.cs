using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Promact.Trappist.Repository.BasicSetup;
using System.Threading.Tasks;
using Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Promact.Trappist.Utility.DbUtil;
using Promact.Trappist.Utility.FileUtil;
using Promact.Trappist.Utility.EmailServices;

namespace Promact.Trappist.Test.BasicSetup
{
    [Collection("Register Dependency")]
    public class BasicSetupRepositoryTest : BaseTest
    {
        #region Private Variables
        #region Dependencies
        private readonly IBasicSetupRepository _basicSetupRepository;
        private readonly Mock<IHostingEnvironment> _hostingEnvironmentMock;
        private readonly Mock<IDbUtility> _sqlConnectionMock;
        private readonly Mock<IFileUtility> _fileUtility;
        private readonly Mock<IEmailService> _emailService;
        private readonly ConnectionString _connectionString;
        #endregion
        #endregion

        #region Constructor
        public BasicSetupRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _basicSetupRepository = _scope.ServiceProvider.GetService<IBasicSetupRepository>();
            _hostingEnvironmentMock = _scope.ServiceProvider.GetService<Mock<IHostingEnvironment>>();
            _sqlConnectionMock = _scope.ServiceProvider.GetService<Mock<IDbUtility>>();
            _fileUtility = _scope.ServiceProvider.GetService<Mock<IFileUtility>>();
            _emailService = _scope.ServiceProvider.GetService<Mock<IEmailService>>();
            _connectionString = _scope.ServiceProvider.GetService<ConnectionString>();
        }
        #endregion

        #region Testing Methods
        /// <summary>
        /// This test case used to check user is created.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateAdminUserTest()
        {
            var basicSetup = InitializeBasicSetupParameters();
            _hostingEnvironmentMock.Setup(x => x.ContentRootPath).Returns(string.Empty);
            _fileUtility.Setup(x => x.WriteJson(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var result = await _basicSetupRepository.CreateAdminUser(basicSetup);
            Assert.True(result.IsSuccess);
        }

        /// <summary>
        /// This test case used to check connection string is valid.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ValidateConnectionStringTest()
        {
            var basicSetup = InitializeBasicSetupParameters();
            _sqlConnectionMock.Setup(x => x.TryOpenSqlConnection(basicSetup.ConnectionString)).Returns(Task.FromResult(true));
            var result = await _basicSetupRepository.ValidateConnectionString(basicSetup.ConnectionString);
            Assert.True(result);
        }

        /// <summary>
        /// This test case used to check email settings are valid.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ValidateEmailSettingTest()
        {
            var basicSetup = InitializeBasicSetupParameters();
            _emailService.Setup(x => x.SendMailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            var result = await _basicSetupRepository.ValidateEmailSetting(basicSetup.EmailSettings);
            Assert.True(result);
        }

        /// <summary>
        /// This test case used to check first time user.
        /// </summary>
        [Fact]
        public void IsFirstTimeUser()
        {
            _connectionString.Value = string.Empty;
            var result = _basicSetupRepository.IsFirstTimeUser();
            Assert.True(result);
        }

        /// <summary>
        /// This test case used to check user is not first time user.
        /// </summary>
        [Fact]
        public void InvalidIsFirstTimeUser()
        {
            var result = _basicSetupRepository.IsFirstTimeUser();
            Assert.False(result);
        }

        /// <summary>
        /// This test case used to check user is not created.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task InvalidCreateAdminUserTest()
        {
            var basicSetup = InitializeBasicSetupParameters();
            _hostingEnvironmentMock.Setup(x => x.ContentRootPath).Returns(string.Empty);
            _fileUtility.Setup(x => x.WriteJson(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            var result = await _basicSetupRepository.CreateAdminUser(basicSetup);
            Assert.False(result.IsSuccess);
        }

        /// <summary>
        /// This test case used to check connection string is invalid
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task InvalidValidateConnectionStringTest()
        {
            var basicSetup = InitializeBasicSetupParameters();
            _sqlConnectionMock.Setup(x => x.TryOpenSqlConnection(basicSetup.ConnectionString)).Returns(Task.FromResult(false));
            var result = await _basicSetupRepository.ValidateConnectionString(basicSetup.ConnectionString);
            Assert.False(result);
        }

        /// <summary>
        /// This test case used to check email settings are invalid.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task InvalidValidateEmailSettingTest()
        {
            var basicSetup = InitializeBasicSetupParameters();
            _emailService.Setup(x => x.SendMailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(false));
            var result = await _basicSetupRepository.ValidateEmailSetting(basicSetup.EmailSettings);
            Assert.False(result);
        }

        /// <summary>
        /// This method used for initialize setup parameters. 
        /// </summary>
        private BasicSetupModel InitializeBasicSetupParameters()
        {
            var basicSetup = new BasicSetupModel();
            basicSetup.ConnectionString = new ConnectionString();
            basicSetup.ConnectionString.Value = "Server=(localdb)\\mssqllocaldb;Database=TrappistDb;Trusted_Connection=True";
            basicSetup.EmailSettings = new EmailSettings();
            basicSetup.EmailSettings.UserName = "xyz@promactinfo.com";
            basicSetup.EmailSettings.Password = "Promact2016";
            basicSetup.EmailSettings.Server = "webmail.promactinfo.com";
            basicSetup.EmailSettings.ConnectionSecurityOption = "None";
            basicSetup.EmailSettings.Port = 587;
            basicSetup.RegistrationFields = new RegistrationFields();
            basicSetup.RegistrationFields.Name = "xyz";
            basicSetup.RegistrationFields.Email = "xyz@promactinfo.com";
            basicSetup.RegistrationFields.Password = "Abc12345@";
            basicSetup.RegistrationFields.ConfirmPassword = "Abc12345@";
            return basicSetup;
        }
        #endregion
    }
}
