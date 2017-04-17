using Promact.Trappist.DomainModel.Models.Question;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Question
{
    public class CodeSnippetQuestionAC
    {
        public bool CheckCodeComplexity { get; set; }

        public bool CheckTimeComplexity { get; set; }

        public bool RunBasicTestCase { get; set; }

        public bool RunCornerTestCase { get; set; }

        public bool RunNecessaryTestCase { get; set; }

        [Required]
        public ICollection<string> LanguageList { get; set; }

        public ICollection<CodeSnippetQuestionTestCases> TestCases { get; set; }
    }
}
