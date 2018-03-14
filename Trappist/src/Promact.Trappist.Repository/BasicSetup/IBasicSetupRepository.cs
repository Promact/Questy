using Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.BasicSetup
{
    public interface IBasicSetupRepository
    {
        /// <summary>
        /// This method used for creating the user, save setup parameter and initialize database.
        /// </summary>
        /// <param name="model">Model object of type BasicSetupModel</param>
        /// <returns>It return response object which has true or false or error message.</returns>
        Task<ServiceResponse> CreateAdminUser(BasicSetupModel model);

        /// <summary>
        /// This method used for validating connection string
        /// </summary>
        /// <param name="model">Model object of type ConnectionString</param>
        /// <returns>If valid then return true else return false</returns>
        Task<bool> ValidateConnectionString(ConnectionString model);

        /// <summary>
        /// This method used for verifying Email settings
        /// </summary>
        /// <param name="model">Model object of type EmailSettings</param>
        /// <returns>If valid email settings then return true else false</returns>
        Task<bool> ValidateEmailSetting(EmailSettings model);

        /// <summary>
        /// This method used for checking SetupConfig.json file exist or not
        /// </summary>
        /// <returns>If exist then return true or return false</returns>
        bool IsFirstTimeUser();
    }
}