using Microsoft.AspNetCore.Identity;
using Promact.Trappist.Web.Models;
using System.Threading.Tasks;
using System;
using System.Data.SqlClient;
using Promact.Trappist.Utility.EmailServices;
using System.IO;
using Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.Utility.Constants;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.Seed;

namespace Promact.Trappist.Repository.BasicSetup
{
    public class BasicSetupRepository : IBasicSetupRepository
    {
        #region Private Variables
        #region Dependencies
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IHostingEnvironment _environment;
        private readonly TrappistDbContext _trappistDbContext;
        private readonly IStringConstants _stringConstants;
        private readonly ConnectionString _connectionString;
        private readonly EmailSettings _emailSettings;
        #endregion
        #endregion

        #region Constructor
        public BasicSetupRepository(UserManager<ApplicationUser> userManager, IEmailService emailService,
            IHostingEnvironment environment, TrappistDbContext trappistDbContext, IStringConstants stringConstants,
            ConnectionString connectionString, EmailSettings emailSettings)
        {
            _userManager = userManager;
            _emailService = emailService;
            _environment = environment;
            _trappistDbContext = trappistDbContext;
            _stringConstants = stringConstants;
            _connectionString = connectionString;
            _emailSettings = emailSettings;
        }
        #endregion

        #region IBasicSetupRepository methods      
        public async Task<bool> CreateAdminUser(BasicSetupModel model)
        {
            var user = new ApplicationUser()
            {
                Name = model.RegistrationFields.Name,
                UserName = model.RegistrationFields.Email,
                Email = model.RegistrationFields.Email,
                CreatedDateTime = DateTime.UtcNow
            };
            bool response = SaveSetupParameter(model);
            _connectionString.Value = model.ConnectionString.Value;
            bool createdUser = await CreateUserAndInitializeDb(user, model.RegistrationFields.Password);
            return response && createdUser;
        }

        /// <summary>
        /// Method used for create user and initialize database
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>if result succeeded than return true else false</returns>
        private async Task<bool> CreateUserAndInitializeDb(ApplicationUser user, string password)
        {
            try
            {
                _trappistDbContext.Database.EnsureDeleted(); //delete if already there -added by roshni
                _trappistDbContext.Database.EnsureCreated(); //create database -added by roshni
                _trappistDbContext.Seed(); //seed -added by roshni

                var result = await _userManager.CreateAsync(user, password);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ValidateConnectionString(ConnectionString model)
        {
            try
            {
                //build connction string
                var builder = new SqlConnectionStringBuilder(model.Value);
                using (var conn = new SqlConnection(GetConnectionString(builder)))
                {
                    try
                    {
                        await conn.OpenAsync();
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// This method used for removing database parameter from the connection string.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>It returns the connection string without database </returns>
        private string GetConnectionString(SqlConnectionStringBuilder connectionString)
        {
            if (connectionString.IntegratedSecurity)
                return string.Format("Data Source={0};Trusted_Connection={1}", connectionString.DataSource, connectionString.IntegratedSecurity);
            else
                return string.Format("Data Source={0};User Id={1};Password={2}", connectionString.DataSource, connectionString.UserID, connectionString.Password);
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
            File.WriteAllText(path, jsonData);
            return true;
        }

        public bool IsFirstTimeUser()
        {
            return string.IsNullOrWhiteSpace(_connectionString.Value);
        }

        /// <summary>
        /// This method is used to retrive setup.json file path.
        /// </summary>
        /// <returns>return path as a string</returns>
        private string GetSetupFilePath()
        {
            return Path.Combine(_environment.ContentRootPath.ToString(), _stringConstants.SetupConfigFileName);
        }
        #endregion
    }
}
