
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Question;

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
                new CodingLanguage {Language = ProgramingLanguage.Java},
                new CodingLanguage {Language = ProgramingLanguage.Cpp},
                new CodingLanguage {Language = ProgramingLanguage.C}
            };

            foreach (CodingLanguage language in Languages)
            {
                dbContext.CodingLanguage.Add(language);
            }

            dbContext.SaveChanges();
        }

        
    }
}
