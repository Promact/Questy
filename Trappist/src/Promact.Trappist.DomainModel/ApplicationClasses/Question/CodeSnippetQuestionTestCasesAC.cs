using Promact.Trappist.DomainModel.Enum;
using System.ComponentModel.DataAnnotations;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Question
{
    public class CodeSnippetQuestionTestCasesAC
    {
        [Required]
        public int Id { get; set; }

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
    }
}
