using Promact.Trappist.DomainModel.Enum;
using System.ComponentModel.DataAnnotations;

namespace Promact.Trappist.DomainModel.Models.Question
{
    public class CodeSnippetQuestionTestCases : BaseModel
    {
        [Required]
        public string TestCaseTitle { get; set; }

        public string TestCaseDescription { get; set; }

        [Required]
        public TestCaseType TestCaseType { get; set; }

        [Required]
        public string TestCaseInput { get; set; }

        [Required]
        public string TestCaseOutput { get; set; }

        [Required]
        public double TestCaseMarks { get; set; }

        public int CodeSnippetQuestionId { get; set; }

        public virtual CodeSnippetQuestion CodeSnippetQuestion { get; set; }
    }
}
