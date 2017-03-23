
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Question;
using System.Linq;

namespace Promact.Trappist.Test
{
    public class ClearDatabase
    {
        public static void ClearDatabaseAndSeed(TrappistDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            //seed data will go here.
            var Languages = new CodingLanguage[]
            {
                new CodingLanguage {Language = ProgrammingLanguage.Java.ToString()},
                new CodingLanguage {Language = ProgrammingLanguage.Cpp.ToString()},
                new CodingLanguage {Language = ProgrammingLanguage.C.ToString()}
            };

            foreach (CodingLanguage language in Languages)
            {
                dbContext.CodingLanguage.Add(language);
            }

            dbContext.SaveChanges();
        }

        
    }
}
