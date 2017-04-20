using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.Test
{
    public class TestQuestion
    {
        [Key]
        public int Id { get; set; }

        public int TestId { get; set; }

        public int QuestionId { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Question.Question Question { get; set; }
       
        [ForeignKey("TestId")]
        public virtual Test Test { get; set; }
    }
}
