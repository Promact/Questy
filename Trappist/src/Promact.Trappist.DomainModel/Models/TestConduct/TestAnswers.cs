using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.TestConduct
{
    public class TestAnswers : BaseModel
    {
        public int TestConductId { get; set; }

        public int? AnsweredOption { get; set; }

        public string AnsweredCodeSnippet { get; set; }

        [ForeignKey("TestConductId")]
        public virtual TestConduct TestConduct { get; set; }
    }
}
