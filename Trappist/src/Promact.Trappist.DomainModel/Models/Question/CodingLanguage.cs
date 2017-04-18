using Promact.Trappist.DomainModel.ApplicationClass;
using System.ComponentModel.DataAnnotations;

namespace Promact.Trappist.DomainModel.Models.Question
{
    public class CodingLanguage: BaseModel
    { 
        [Required]
        public ProgramingLanguageEnum Language { get; set; }

        public virtual QuestionLanguageMapping QuestionLanguangeMapping { get; set; }

    }
}
