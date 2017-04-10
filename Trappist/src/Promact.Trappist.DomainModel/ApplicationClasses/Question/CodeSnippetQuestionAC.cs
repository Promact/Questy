using Promact.Trappist.DomainModel.Models.Question;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Question
{
    public class CodeSnippetQuestionAC : CodeSnippetQuestion
    {
        [Required]
        public ICollection<string> LanguageList { get; set; }
        public ICollection<CodeSnippetQuestionTestCases> TestCases { get; set; }
    }
}
