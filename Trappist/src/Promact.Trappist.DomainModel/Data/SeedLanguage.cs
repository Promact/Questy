using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.DomainModel.Data
{
    public class SeedLanguage
    {
        public static void Seed(TrappistDbContext context) {
            context.Database.EnsureCreated();

            if (context.CodingLanguage.Any())
            {
                return;
            }

            var Languages = new CodingLanguage[]
            {
                new CodingLanguage {Language = "Java",  CreatedDateTime = DateTime.UtcNow, UpdateDateTime = DateTime.UtcNow },
                new CodingLanguage {Language = "C++",  CreatedDateTime = DateTime.UtcNow, UpdateDateTime = DateTime.UtcNow  },
                new CodingLanguage {Language = "C",  CreatedDateTime = DateTime.UtcNow, UpdateDateTime = DateTime.UtcNow  }
            };

            foreach(CodingLanguage language in Languages)
            {
                context.CodingLanguage.Add(language);
            }

            context.SaveChanges();
        }
    }
}
