using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Promact.Trappist.DomainModel.Models.Question
{
    public class CodingLanguage: BaseModel
    { 
        [Required]
        public string Language { get; set; }

        public virtual ICollection<QuestionLanguageMapping> QuestionLanguangeMapping { get; set; }
    }
}
