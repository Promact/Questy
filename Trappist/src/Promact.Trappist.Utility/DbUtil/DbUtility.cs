using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Seed;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Promact.Trappist.Utility.DbUtil
{
    public class DbUtility : IDbUtility
    {
        #region Private Variables
        #region Dependencies
        private readonly TrappistDbContext _trappistDbContext;
        private readonly IHostingEnvironment _hostingEnv;
        #endregion
        #endregion

        #region Constructor
        public DbUtility(TrappistDbContext trappistDbContext,IHostingEnvironment hostingEnv)
        {
            _trappistDbContext = trappistDbContext;
            _hostingEnv = hostingEnv;
        }
        #endregion

        #region IDbUtility methods
        public async Task<bool> TryOpenSqlConnection(ConnectionString model)
        {
            try
            {
                //For building a connection string
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

        public void MigrateAndSeedDb()
        {
            if (_hostingEnv.IsProduction())
                _trappistDbContext.Database.EnsureDeleted();
            _trappistDbContext.Database.Migrate();
            _trappistDbContext.Seed();
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
        #endregion
    }
}
