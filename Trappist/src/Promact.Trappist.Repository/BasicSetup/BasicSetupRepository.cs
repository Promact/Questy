
using Promact.Trappist.Web.Models;
using System.Threading.Tasks;
using System;
using Promact.Trappist.Utility.EmailServices;
using System.IO;
using Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Promact.Trappist.Utility.Constants;
using Promact.Trappist.Utility.DbUtil;
using Promact.Trappist.Utility.FileUtil;
using Microsoft.AspNetCore.Identity;

namespace Promact.Trappist.Repository.BasicSetup
{
    public class BasicSetupRepository : IBasicSetupRepository
    {
        #region Private Variables
        #region Dependencies
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IHostingEnvironment _environment;
        private readonly IStringConstants _stringConstants;
        private readonly ConnectionString _connectionString;
        private readonly EmailSettings _emailSettings;
        private readonly IDbUtility _dbUtility;
        private readonly IFileUtility _fileUtility;
        #endregion
        #endregion

        #region Constructor
        public BasicSetupRepository(UserManager<ApplicationUser> userManager, IEmailService emailService,
            IHostingEnvironment environment, IStringConstants stringConstants,
            ConnectionString connectionString, EmailSettings emailSettings, IDbUtility dbUtility, IFileUtility fileUtility)
        {
            _userManager = userManager;
            _emailService = emailService;
            _environment = environment;
            _stringConstants = stringConstants;
            _connectionString = connectionString;
            _emailSettings = emailSettings;
            _dbUtility = dbUtility;
            _fileUtility = fileUtility;
        }
        #endregion

        #region IBasicSetupRepository methods
        #region Public Methods
        public async Task<ServiceResponse> CreateAdminUser(BasicSetupModel model)
        {
            var user = new ApplicationUser()
            {
                Name = model.RegistrationFields.Name,
                UserName = model.RegistrationFields.Email,
                Email = model.RegistrationFields.Email,
                CreatedDateTime = DateTime.UtcNow
            };
            _connectionString.Value = model.ConnectionString.Value;
            var response = await CreateUserAndInitializeDb(user, model.RegistrationFields.Password);
            if (response.IsSuccess)
            {
                response.IsSuccess = SaveSetupParameter(model);
                return response;
            }
            else
            {
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<bool> ValidateConnectionString(ConnectionString model)
        {
            return await _dbUtility.TryOpenSqlConnection(model);
        }

        public async Task<bool> ValidateEmailSetting(EmailSettings model)
        {
            _emailSettings.UserName = model.UserName;
            _emailSettings.Password = model.Password;
            _emailSettings.Server = model.Server;
            _emailSettings.Port = model.Port;
            _emailSettings.ConnectionSecurityOption = model.ConnectionSecurityOption;
            return await _emailService.SendMailAsync(_emailSettings.UserName, _emailSettings.UserName, _stringConstants.TestMailBody, _stringConstants.TestMailSubject);
        }

        public bool IsFirstTimeUser()
        {
            return string.IsNullOrWhiteSpace(_connectionString.Value);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Method used for create user and initialize database
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>It return true or false or error message in response object.</returns>
        private async Task<ServiceResponse> CreateUserAndInitializeDb(ApplicationUser user, string password)
        {
            var response = new ServiceResponse();
            try
            {
                _dbUtility.MigrateAndSeedDb(); //Seed and migrate database.
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    response.ExceptionMessage = _stringConstants.UserAlreadyExistErrorMessage;
                    response.IsSuccess = false;
                }
                else
                    response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ExceptionMessage = ex.Message;
                return response;
            }
        }

        /// <summary>
        /// This method used for saving setup parameter in Setup.json file
        /// </summary>
        /// <param name="model"></param>
        /// <returns>It return true or false</returns>
        private bool SaveSetupParameter(BasicSetupModel model)
        {
            var setupParameter = new Setup()
            {
                ConnectionString = model.ConnectionString,
                EmailSettings = model.EmailSettings
            };
            string jsonData = JsonConvert.SerializeObject(setupParameter, Formatting.Indented);
            string path = GetSetupFilePath();
            return _fileUtility.WriteJson(path, jsonData);
        }

        /// <summary>
        /// This method is used to retrieve setup.json file path.
        /// </summary>
        /// <returns>It return path as a string</returns>
        private string GetSetupFilePath()
        {
            return Path.Combine(_environment.ContentRootPath.ToString(), _stringConstants.SetupConfigFileName);
        }
        #endregion
        #endregion
    }
}
