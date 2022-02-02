using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Seed;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Npgsql;

namespace Promact.Trappist.Utility.DbUtil
{
    [ExcludeFromCodeCoverage]
    public class DbUtility : IDbUtility
    {
        #region Private Variables
        #region Dependencies
        private readonly TrappistDbContext _trappistDbContext;
        #endregion
        #endregion

        #region Constructor
        public DbUtility(TrappistDbContext trappistDbContext)
        {
            _trappistDbContext = trappistDbContext;
        }
        #endregion

        #region IDbUtility methods
        public async Task<bool> TryOpenSqlConnection(ConnectionString model)
        {
            try
            {
                //For building a connection string
                var builder = new NpgsqlConnectionStringBuilder(model.Value);
                await using var conn = new NpgsqlConnection(GetConnectionString(builder));
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
            catch (Exception)
            {
                return false;
            }
        }

        public void MigrateAndSeedDb()
        {          
            _trappistDbContext.Database.Migrate();
            _trappistDbContext.Seed();
        }

        /// <summary>
        /// This method used for removing database parameter from the connection string.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>It returns the connection string without database </returns>
        private string GetConnectionString(NpgsqlConnectionStringBuilder connectionString)
        {
            //Server=localhost;Port=5432;Database=questy-prod;User Id=postgres;Password=1b0d3bbf22eb4bf7be446ef03ebc0ad1;
            return
                    $"User Id={connectionString.Username};Password={connectionString.Password};Server={connectionString.Host}";
        }
        #endregion
    }
}
