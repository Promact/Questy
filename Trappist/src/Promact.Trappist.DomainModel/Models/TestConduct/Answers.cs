using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.TestConduct
{
    public class Answers : BaseModel
    {
        public int TestConductId { get; set; }

        public int AnsweredOption { get; set; }

        public string AnsweredCodeSnippet { get; set; }

        [ForeignKey("TestConductId")]
        public virtual TestConduct TestCoduct { get; set; }
    }
}
