using Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup;
using System.Threading.Tasks;

namespace Promact.Trappist.Utility.DbUtil
{
    public interface IDbUtility
    {
        /// <summary>
        /// This method used for validate connection string.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>return true if valid connection string else return false</returns>
        Task<bool> TryOpenSqlConnection(ConnectionString model);

        /// <summary>
        /// This method used for migrate and seed database.
        /// </summary>
        void MigrateAndSeedDb();
    }
}
