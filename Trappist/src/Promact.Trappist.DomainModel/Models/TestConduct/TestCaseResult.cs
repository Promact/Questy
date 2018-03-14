using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.TestConduct
{
    /// <remarks>
    /// TestCaseResult cannot have direct relationship with CodeSnippetQuestionTestCases table because of 
    /// circular dependency. This has been overcomed by adding CodeSnippetQuestionTestCasesId property. 
    /// </remarks>
    public class TestCaseResult: BaseModel
    {
        public long Processing { get; set; }

        public long Memory { get; set; }
        
        public string Output { get; set; }

        public int CodeSnippetQuestionTestCasesId { get; set; }

        public int TestCodeSolutionId { get; set; }

        [ForeignKey("TestCodeSolutionId")]
        public virtual TestCodeSolution TestCodeSolution { get; set; }
    }
}
