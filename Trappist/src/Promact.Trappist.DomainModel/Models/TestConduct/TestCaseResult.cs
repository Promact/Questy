using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.TestConduct
{
    public class TestCaseResult: BaseModel
    {
        public long Processing { get; set; }

        public long Memory { get; set; }

        public string Output { get; set; }

        public int TestCodeSolutionId { get; set; }

        [ForeignKey("TestCodeSolutionId")]
        public virtual TestCodeSolution TestCodeSolution { get; set; }
    }
}
