using System.Collections.Generic;

namespace Promact.Trappist.DomainModel.Models.Question
{
    public class CodeSnippetQuestion : QuestionBase
    {
        public bool CheckCodeComplexity { get; set; }

        public bool CheckTimeComplexity { get; set; }

        public bool RunBasicTestCase { get; set; }

        public bool RunCornerTestCase { get; set; }

        public bool RunNecessaryTestCase { get; set; }

        public virtual ICollection<QuestionLanguageMapping> QuestionLanguangeMapping { get; set; }
    }
}
