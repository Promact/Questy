using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.Question
{
    public class CodeSnippetQuestion
    {
        [ForeignKey("Question")]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }

        public bool CheckCodeComplexity { get; set; }

        public bool CheckTimeComplexity { get; set; }

        public bool RunBasicTestCase { get; set; }

        public bool RunCornerTestCase { get; set; }

        public bool RunNecessaryTestCase { get; set; }

        public virtual ICollection<QuestionLanguageMapping> QuestionLanguangeMapping { get; set; }

        public virtual ICollection<CodeSnippetQuestionTestCases> CodeSnippetQuestionTestCases { get; set; }

        public virtual Question Question { get; set; }
    }
}
