using System.Diagnostics.CodeAnalysis;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Question;
using System.Linq;

namespace Promact.Trappist.DomainModel.Seed
{ 
    /// <summary>
    /// Populates pre-required table
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class SeedDatabaseExtensions
    {
        /// <summary>
        /// Seed data into the table
        /// </summary>
        /// <param name="context">DbContext</param>
        public static void Seed(this TrappistDbContext context) {

            if (context.CodingLanguage.Any())
            {
                return;
            }

            var languages = new CodingLanguage[]
            {
                new() {Language = ProgrammingLanguage.Java.ToString()},
                new() {Language = ProgrammingLanguage.Cpp.ToString()},
                new() {Language = ProgrammingLanguage.C.ToString()}
            };

            foreach(CodingLanguage language in languages)
            {
                context.CodingLanguage.Add(language);
            }

            context.SaveChanges();
        }
    }
}
