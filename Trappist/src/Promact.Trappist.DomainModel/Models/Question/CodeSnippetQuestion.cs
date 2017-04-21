using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.Question
{
    public class CodeSnippetQuestion: BaseModel
    {
        public bool CheckCodeComplexity { get; set; }

        public bool CheckTimeComplexity { get; set; }

        public bool RunBasicTestCase { get; set; }

        public bool RunCornerTestCase { get; set; }

        public bool RunNecessaryTestCase { get; set; }

        public virtual ICollection<QuestionLanguageMapping> QuestionLanguangeMapping { get; set; }

        public virtual ICollection<CodeSnippetQuestionTestCases> CodeSnippetQuestionTestCases { get; set; }

        [ForeignKey("Id")]
        public virtual Question Question { get; set; }
    }
}
