using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.DomainModel.Data
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
            context.Database.EnsureCreated();

            if (context.CodingLanguage.Any())
            {
                return;
            }

            var Languages = new CodingLanguage[]
            {
                new CodingLanguage {Language = language.java,  CreatedDateTime = DateTime.UtcNow, UpdateDateTime = DateTime.UtcNow },
                new CodingLanguage {Language = language.cpp,  CreatedDateTime = DateTime.UtcNow, UpdateDateTime = DateTime.UtcNow  },
                new CodingLanguage {Language = language.c,  CreatedDateTime = DateTime.UtcNow, UpdateDateTime = DateTime.UtcNow  }
            };

            foreach(CodingLanguage language in Languages)
            {
                context.CodingLanguage.Add(language);
            }

            context.SaveChanges();
        }
    }
}
