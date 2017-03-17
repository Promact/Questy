

using Promact.Trappist.DomainModel.DbContext;

namespace Promact.Trappist.Test
{
    public class ClearDatabase
    {
        public static void ClearDatabaseAndSeed(TrappistDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            //seed data will go here.
        }
    }
}
