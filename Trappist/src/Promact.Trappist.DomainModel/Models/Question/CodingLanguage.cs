using Promact.Trappist.DomainModel.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Promact.Trappist.DomainModel.Models.Question
{
    public class CodingLanguage: BaseModel
    { 
        [Required]
        public ProgramingLanguageEnum Language { get; set; }

        public virtual ICollection<QuestionLanguageMapping> QuestionLanguangeMapping { get; set; }
    }
}
