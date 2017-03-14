using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Question;
using System.Linq;

namespace Promact.Trappist.DomainModel.Seed
{ 
    /// <summary>
    /// Polpulates pre-required table
    /// </summary>
    public class SeedDatabase
    {
        /// <summary>
        /// Seed data into the table
        /// </summary>
        /// <param name="context">DbContext</param>
        public static void Seed(TrappistDbContext context) {

            if (context.CodingLanguage.Any())
            {
                return;
            }

            var Languages = new CodingLanguage[]
            {
                new CodingLanguage {Language = ProgramingLanguage.Java},
                new CodingLanguage {Language = ProgramingLanguage.Cpp},
                new CodingLanguage {Language = ProgramingLanguage.C}
            };

            foreach(CodingLanguage language in Languages)
            {
                context.CodingLanguage.Add(language);
            }

            context.SaveChanges();
        }
    }
}
