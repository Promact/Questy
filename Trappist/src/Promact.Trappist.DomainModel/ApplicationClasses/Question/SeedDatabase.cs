using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.Web.Data;
using System.Linq;

namespace Promact.Trappist.DomainModel.Data
{

    /// <summary>
    /// Polpulates pre-required table
    /// </summary>
    public class SeedDatabase
    {
        public static object ProgramingLanguage { get; private set; }

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
                new CodingLanguage {Language = ProgramingLanguageEnum.Java},
                new CodingLanguage {Language = ProgramingLanguageEnum.Cpp},
                new CodingLanguage {Language = ProgramingLanguageEnum.C}
            };

            foreach(CodingLanguage language in Languages)
            {
                context.CodingLanguage.Add(language);
            }

            context.SaveChanges();
        }
    }
}
