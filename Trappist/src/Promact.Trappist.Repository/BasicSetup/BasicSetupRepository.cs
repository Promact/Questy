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
        #endregion
        #endregion

        #region Constructor
        public BasicSetupRepository(UserManager<ApplicationUser> userManager, IEmailService emailService,
            IHostingEnvironment environment, TrappistDbContext trappistDbContext, IStringConstants stringConstants,
            ConnectionString connectionString)
        {
            _userManager = userManager;
            _emailService = emailService;
            _environment = environment;
            _trappistDbContext = trappistDbContext;
            _stringConstants = stringConstants;
            _connectionString = connectionString;
        }
        #endregion

        #region IBasicSetupRepository methods
        /// <summary>
        /// This method used for creating the user, save setup parameter and initialize database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>if a result succeeded then return true else return false</returns>
        public async Task<bool> CreateAdminUser(BasicSetupModel model)
        {
            var user = new ApplicationUser()
            {
                Name = model.RegistrationFields.Name,
                UserName = model.RegistrationFields.Email,
                Email = model.RegistrationFields.Email,
                CreateDateTime = DateTime.UtcNow
            };
            bool response = SaveSetupParameter(model);
            _connectionString.Value = model.ConnectionString.Value;
            bool createdUser = await CreateUserAndInitializeDb(user, model.RegistrationFields.Password);
            if (response && createdUser)
                return true;
            return false;
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
                _trappistDbContext.Database.EnsureCreated();
                _trappistDbContext.Database.Migrate();
                _trappistDbContext.Seed();
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// This method used for validating connection string
        /// </summary>
        /// <param name="model"></param>
        /// <returns>If valid then return true else return false</returns>
        public bool ValidateConnectionString(ConnectionString model)
        {
            try
            {
                //build connction string
                var builder = new SqlConnectionStringBuilder(model.Value);
                using (var conn = new SqlConnection(GetConnectionString(builder)))
                {
                    try
                    {
                        conn.Open();
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

        /// <summary>
        /// This method used for verifying Email settings
        /// </summary>
        /// <param name="model"></param>
        /// <returns>if valid email settings then return true else false</returns>
        public Task<bool> ValidateEmailSetting(EmailSettings model)
        {
            return _emailService.SendMail(model.UserName, model.Password, model.Server, model.Port, model.ConnectionSecurityOption, _stringConstants.BodyOfMail , model.UserName);
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

        /// <summary>
        /// This method used for checking user first time entered or not.
        /// </summary>
        /// <returns>If user first time enter then return true else return false</returns>
        public bool IsFirstTimeUser()
        {
            return !string.IsNullOrWhiteSpace(_connectionString.Value);
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
