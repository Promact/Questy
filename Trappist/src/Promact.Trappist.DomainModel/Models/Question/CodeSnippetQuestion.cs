using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Promact.Trappist.DomainModel.Models.Question
{
    //To-Do inherit QuestionBaseModel instead of BaseModel once it is created
    public class CodeSnippetQuestion : QuestionBase
    {
        [Required]
        public bool CheckCodeComplexity { get; set; }

        [Required]
        public bool CheckTimeComplexity { get; set; }

        [Required]
        public bool RunBasicTestCase { get; set; }

        [Required]
        public bool RunCornerTestCase { get; set; }

        [Required]
        public bool RunNecessaryTestCase { get; set; }

        public virtual ICollection<QuestionLanguageMapping> QuestionLanguangeMapping { get; set; }
    }
}
